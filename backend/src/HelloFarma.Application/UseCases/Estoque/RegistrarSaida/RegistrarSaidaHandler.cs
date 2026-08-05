using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarSaida;

/// <summary>
/// Baixa estoque seguindo a regra FEFO (First Expire, First Out) — sempre consome
/// primeiro o lote com validade mais próxima, reduzindo perdas por vencimento.
/// Se a quantidade solicitada não couber em um único lote, distribui entre lotes.
/// </summary>
public class RegistrarSaidaHandler(
    ILoteRepository loteRepository,
    IMovimentacaoEstoqueRepository movimentacaoRepository,
    ICurrentTenant currentTenant,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarSaidaCommand, Unit>
{
    public async Task<Unit> Handle(RegistrarSaidaCommand request, CancellationToken ct)
    {
        var lotes = (await loteRepository.ListarPorProdutoAsync(request.ProdutoId, ct))
            .Where(l => l.QuantidadeAtual > 0)
            .OrderBy(l => l.Validade)
            .ToList();

        var totalDisponivel = lotes.Sum(l => l.QuantidadeAtual);
        if (totalDisponivel < request.Quantidade)
            throw new InvalidOperationException("Estoque insuficiente para realizar a saída (ruptura).");

        var restante = request.Quantidade;

        foreach (var lote in lotes)
        {
            if (restante <= 0) break;

            var quantidadeDoLote = Math.Min(lote.QuantidadeAtual, restante);
            lote.Baixar(quantidadeDoLote);
            loteRepository.Update(lote);

            var movimentacao = new MovimentacaoEstoque(
                currentTenant.TenantId, request.ProdutoId, lote.Id, TipoMovimentacaoEstoque.Saida, quantidadeDoLote, request.Motivo ?? "Saída de estoque");
            await movimentacaoRepository.AddAsync(movimentacao, ct);

            restante -= quantidadeDoLote;
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
