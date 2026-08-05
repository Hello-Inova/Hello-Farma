namespace HelloFarma.Application.DTOs;

public record LogAuditoriaDto(
    Guid Id,
    Guid UsuarioId,
    string? UsuarioNome,
    string Acao,
    bool Sucesso,
    string? Erro,
    string? IpAddress,
    DateTime CreatedAtUtc);
