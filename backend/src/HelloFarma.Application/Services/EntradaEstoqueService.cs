using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Application.Services;

public class EntradaEstoqueService(
    ILoteRepository loteRepository,
    IMovimentacaoEstoqueRepository movimentacaoRepository,
    ICurrentTenant currentTenant) : IEntradaEstoqueService
{
    public async Task<Guid> EntrarAsync(Guid produtoId, string numeroLote, DateOnly validade, int quantidade, string? localizacao, string motivo, Guid? filialId = null, CancellationToken ct = default)
    {
        var lote = await loteRepository.ObterPorNumeroAsync(produtoId, numeroLote, filialId, ct);

        if (lote is null)
        {
            lote = new Lote(currentTenant.TenantId, produtoId, numeroLote, validade, quantidade, localizacao, filialId);
            await loteRepository.AddAsync(lote, ct);
        }
        else
        {
            lote.Adicionar(quantidade);
            loteRepository.Update(lote);
        }

        var movimentacao = new MovimentacaoEstoque(currentTenant.TenantId, produtoId, lote.Id, TipoMovimentacaoEstoque.Entrada, quantidade, motivo);
        await movimentacaoRepository.AddAsync(movimentacao, ct);

        return lote.Id;
    }
}
