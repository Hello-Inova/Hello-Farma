using MediatR;

namespace HelloFarma.Application.UseCases.Delivery.CriarPedidoDelivery;

public record CriarPedidoDeliveryCommand(Guid VendaId, string EnderecoEntrega, Guid? ClienteId) : IRequest<Guid>;
