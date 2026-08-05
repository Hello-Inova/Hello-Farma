using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.ListarLotesProximosVencimento;

public class ListarLotesProximosVencimentoHandler(ILoteRepository loteRepository, IProdutoRepository produtoRepository)
    : IRequestHandler<ListarLotesProximosVencimentoQuery, IReadOnlyList<LoteDto>>
{
    public async Task<IReadOnlyList<LoteDto>> Handle(ListarLotesProximosVencimentoQuery request, CancellationToken ct)
    {
        var lotes = await loteRepository.ListarProximosDoVencimentoAsync(request.DiasAlerta, ct);
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        var resultado = new List<LoteDto>();
        foreach (var lote in lotes.OrderBy(l => l.Validade))
        {
            var produto = await produtoRepository.GetByIdAsync(lote.ProdutoId, ct);
            resultado.Add(new LoteDto(
                lote.Id, lote.ProdutoId, produto?.Nome ?? "(produto removido)", lote.NumeroLote,
                lote.Validade, lote.QuantidadeAtual, lote.Localizacao,
                lote.Validade.DayNumber - hoje.DayNumber));
        }

        return resultado;
    }
}
