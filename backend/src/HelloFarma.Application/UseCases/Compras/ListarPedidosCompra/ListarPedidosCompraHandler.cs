using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.ListarPedidosCompra;

public class ListarPedidosCompraHandler(IPedidoCompraRepository pedidoRepository)
    : IRequestHandler<ListarPedidosCompraQuery, IReadOnlyList<PedidoCompraDto>>
{
    public async Task<IReadOnlyList<PedidoCompraDto>> Handle(ListarPedidosCompraQuery request, CancellationToken ct)
    {
        var pedidos = await pedidoRepository.ListarAsync(ct);

        return pedidos.Select(p => new PedidoCompraDto(
            p.Id, p.FornecedorId, (int)p.Status, p.ValorTotal, p.RecebidoEmUtc,
            p.Itens.Select(i => new ItemPedidoCompraDto(i.ProdutoId, i.ProdutoNome, i.Quantidade, i.PrecoUnitario, i.Subtotal, i.NumeroLote, i.Validade)).ToList())).ToList();
    }
}
