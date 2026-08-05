using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarEntrada;

public class RegistrarEntradaHandler(
    ILoteRepository loteRepository,
    IMovimentacaoEstoqueRepository movimentacaoRepository,
    ICurrentTenant currentTenant,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarEntradaCommand, Guid>
{
    public async Task<Guid> Handle(RegistrarEntradaCommand request, CancellationToken ct)
    {
        var lote = await loteRepository.ObterPorNumeroAsync(request.ProdutoId, request.NumeroLote, ct);

        if (lote is null)
        {
            lote = new Lote(currentTenant.TenantId, request.ProdutoId, request.NumeroLote, request.Validade, request.Quantidade, request.Localizacao);
            await loteRepository.AddAsync(lote, ct);
        }
        else
        {
            lote.Adicionar(request.Quantidade);
            loteRepository.Update(lote);
        }

        var movimentacao = new MovimentacaoEstoque(
            currentTenant.TenantId, request.ProdutoId, lote.Id, TipoMovimentacaoEstoque.Entrada, request.Quantidade, request.Motivo ?? "Entrada de estoque");
        await movimentacaoRepository.AddAsync(movimentacao, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return lote.Id;
    }
}
