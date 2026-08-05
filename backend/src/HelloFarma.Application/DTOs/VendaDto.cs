namespace HelloFarma.Application.DTOs;

public record ItemVendaDto(Guid ProdutoId, string ProdutoNome, int Quantidade, decimal PrecoUnitario, decimal Subtotal);

public record VendaDto(
    Guid Id,
    DateTime RealizadaEmUtc,
    int FormaPagamento,
    int Status,
    decimal ValorTotal,
    IReadOnlyList<ItemVendaDto> Itens);
