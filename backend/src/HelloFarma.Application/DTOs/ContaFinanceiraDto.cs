namespace HelloFarma.Application.DTOs;

public record ContaFinanceiraDto(
    Guid Id, int Tipo, string Descricao, decimal Valor, DateOnly DataVencimento,
    DateTime? PagaEmUtc, int Status);

public record FluxoCaixaDto(decimal TotalEntradas, decimal TotalSaidas, decimal Saldo, DateTime InicioUtc, DateTime FimUtc);
