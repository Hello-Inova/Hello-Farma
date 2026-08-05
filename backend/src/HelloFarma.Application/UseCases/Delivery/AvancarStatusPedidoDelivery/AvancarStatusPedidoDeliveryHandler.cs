using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.AvancarStatusPedidoDelivery;

public class AvancarStatusPedidoDeliveryHandler(IPedidoDeliveryRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<AvancarStatusPedidoDeliveryCommand, Unit>
{
    public async Task<Unit> Handle(AvancarStatusPedidoDeliveryCommand request, CancellationToken ct)
    {
        var pedido = await repository.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Pedido de delivery não encontrado.");
        pedido.AvancarPara((StatusPedidoDelivery)request.NovoStatus);
        repository.Update(pedido);
        await unitOfWork.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
