using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Delivery;
using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.CriarPedidoDelivery;

public class CriarPedidoDeliveryHandler(IPedidoDeliveryRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarPedidoDeliveryCommand, Guid>
{
    public async Task<Guid> Handle(CriarPedidoDeliveryCommand request, CancellationToken ct)
    {
        var pedido = new PedidoDelivery(currentTenant.TenantId, request.VendaId, request.EnderecoEntrega, request.ClienteId);
        await repository.AddAsync(pedido, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return pedido.Id;
    }
}
