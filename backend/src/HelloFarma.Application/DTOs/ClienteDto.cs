namespace HelloFarma.Application.DTOs;

public record ClienteDto(Guid Id, string Nome, string? Cpf, string? Telefone, string? Email, decimal SaldoCashback);
