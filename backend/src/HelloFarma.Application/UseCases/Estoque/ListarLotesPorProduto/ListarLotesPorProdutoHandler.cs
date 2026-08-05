using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.ListarLotesPorProduto;

public class ListarLotesPorProdutoHandler(ILoteRepository loteRepository, IProdutoRepository produtoRepository)
    : IRequestHandler<ListarLotesPorProdutoQuery, IReadOnlyList<LoteDto>>
{
    public async Task<IReadOnlyList<LoteDto>> Handle(ListarLotesPorProdutoQuery request, CancellationToken ct)
    {
        var lotes = await loteRepository.ListarPorProdutoAsync(request.ProdutoId, ct);
        var produto = await produtoRepository.GetByIdAsync(request.ProdutoId, ct);
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        return lotes.OrderBy(l => l.Validade).Select(l => new LoteDto(
            l.Id, l.ProdutoId, produto?.Nome ?? "-", l.NumeroLote, l.Validade, l.QuantidadeAtual, l.Localizacao,
            l.Validade.DayNumber - hoje.DayNumber)).ToList();
    }
}
