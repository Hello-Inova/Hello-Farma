using MediatR;

namespace HelloFarma.Application.UseCases.Compras.CriarPedidoCompra;

public record ItemPedidoCompraInput(Guid ProdutoId, int Quantidade, decimal PrecoUnitario, string NumeroLote, DateOnly Validade);

/// <summary>
/// Cria um pedido de compra em cotação, adiciona os itens e já confirma o pedido
/// (fluxo simplificado: Cotação + Pedido em uma única chamada).
/// </summary>
public record CriarPedidoCompraCommand(Guid FornecedorId, List<ItemPedidoCompraInput> Itens) : IRequest<Guid>;
