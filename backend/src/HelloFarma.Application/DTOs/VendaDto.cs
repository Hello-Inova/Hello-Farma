namespace HelloFarma.Application.DTOs;

public record ItemVendaDto(Guid ProdutoId, string ProdutoNome, int Quantidade, decimal PrecoUnitario, decimal Subtotal);

public record VendaDto(
    Guid Id,
    DateTime RealizadaEmUtc,
    int FormaPagamento,
    int Status,
    Guid? ClienteId,
    decimal ValorTotal,
    decimal CashbackUtilizado,
    decimal CashbackGerado,
    decimal ValorPago,
    IReadOnlyList<ItemVendaDto> Itens);
