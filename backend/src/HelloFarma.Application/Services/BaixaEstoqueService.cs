using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Application.Services;

public class BaixaEstoqueService(
    ILoteRepository loteRepository,
    IMovimentacaoEstoqueRepository movimentacaoRepository,
    ICurrentTenant currentTenant) : IBaixaEstoqueService
{
    public async Task BaixarAsync(Guid produtoId, int quantidade, string motivo, Guid? filialId = null, CancellationToken ct = default)
    {
        var lotes = (await loteRepository.ListarPorProdutoAsync(produtoId, filialId, ct))
            .Where(l => l.QuantidadeAtual > 0)
            .OrderBy(l => l.Validade)
            .ToList();

        var totalDisponivel = lotes.Sum(l => l.QuantidadeAtual);
        if (totalDisponivel < quantidade)
            throw new InvalidOperationException("Estoque insuficiente (ruptura).");

        var restante = quantidade;

        foreach (var lote in lotes)
        {
            if (restante <= 0) break;

            var quantidadeDoLote = Math.Min(lote.QuantidadeAtual, restante);
            lote.Baixar(quantidadeDoLote);
            loteRepository.Update(lote);

            var movimentacao = new MovimentacaoEstoque(
                currentTenant.TenantId, produtoId, lote.Id, TipoMovimentacaoEstoque.Saida, quantidadeDoLote, motivo);
            await movimentacaoRepository.AddAsync(movimentacao, ct);

            restante -= quantidadeDoLote;
        }
    }
}
