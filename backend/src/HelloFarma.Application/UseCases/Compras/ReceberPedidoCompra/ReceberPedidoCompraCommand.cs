using MediatR;

namespace HelloFarma.Application.UseCases.Compras.ReceberPedidoCompra;

/// <summary>
/// Confirma o recebimento físico do pedido: dá entrada em estoque de cada item
/// (cria/atualiza os lotes correspondentes) e avança o status para Recebido.
/// </summary>
public record ReceberPedidoCompraCommand(Guid PedidoCompraId) : IRequest<Unit>;
