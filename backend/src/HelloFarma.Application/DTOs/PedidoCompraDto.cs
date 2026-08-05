namespace HelloFarma.Application.DTOs;

public record ItemPedidoCompraDto(Guid ProdutoId, string ProdutoNome, int Quantidade, decimal PrecoUnitario, decimal Subtotal, string NumeroLote, DateOnly Validade);

public record PedidoCompraDto(Guid Id, Guid FornecedorId, int Status, decimal ValorTotal, DateTime? RecebidoEmUtc, IReadOnlyList<ItemPedidoCompraDto> Itens);
