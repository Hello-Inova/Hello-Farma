using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.AvancarStatusPedidoDelivery;

public record AvancarStatusPedidoDeliveryCommand(Guid Id, int NovoStatus) : IRequest<Unit>;
