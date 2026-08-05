namespace HelloFarma.Application.DTOs;

public record FilialDto(
    Guid Id,
    string Nome,
    string? Cnpj,
    string? Endereco,
    string? Cidade,
    string? Uf,
    bool Ativa,
    bool Matriz);
