using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.ListarVendasDoDia;

public class ListarVendasDoDiaHandler(IVendaRepository vendaRepository) : IRequestHandler<ListarVendasDoDiaQuery, IReadOnlyList<VendaDto>>
{
    public async Task<IReadOnlyList<VendaDto>> Handle(ListarVendasDoDiaQuery request, CancellationToken ct)
    {
        var inicio = DateTime.UtcNow.Date;
        var fim = inicio.AddDays(1);

        var vendas = await vendaRepository.ListarPorPeriodoAsync(inicio, fim, ct);

        return vendas.Select(v => new VendaDto(
            v.Id, v.RealizadaEmUtc, (int)v.FormaPagamento, (int)v.Status, v.ValorTotal,
            v.Itens.Select(i => new ItemVendaDto(i.ProdutoId, i.ProdutoNome, i.Quantidade, i.PrecoUnitario, i.Subtotal)).ToList())).ToList();
    }
}
