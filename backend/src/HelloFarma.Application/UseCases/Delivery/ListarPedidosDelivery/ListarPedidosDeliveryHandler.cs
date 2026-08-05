using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.ListarPedidosDelivery;

public class ListarPedidosDeliveryHandler(IPedidoDeliveryRepository repository) : IRequestHandler<ListarPedidosDeliveryQuery, IReadOnlyList<PedidoDeliveryDto>>
{
    public async Task<IReadOnlyList<PedidoDeliveryDto>> Handle(ListarPedidosDeliveryQuery request, CancellationToken ct)
    {
        var pedidos = await repository.ListarEmAndamentoAsync(ct);
        return pedidos.Select(p => new PedidoDeliveryDto(p.Id, p.VendaId, p.EnderecoEntrega, (int)p.Status, p.EntregadorId, p.AvaliacaoNota)).ToList();
    }
}
