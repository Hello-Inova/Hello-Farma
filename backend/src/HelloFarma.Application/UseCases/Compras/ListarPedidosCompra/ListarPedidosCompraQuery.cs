using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.ListarPedidosCompra;

public record ListarPedidosCompraQuery : IRequest<IReadOnlyList<PedidoCompraDto>>;
