using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.ListarPedidosDelivery;

public record ListarPedidosDeliveryQuery : IRequest<IReadOnlyList<PedidoDeliveryDto>>;
