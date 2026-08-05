namespace HelloFarma.Application.DTOs;

public record TenantPlataformaDto(
    Guid Id,
    string RazaoSocial,
    string NomeFantasia,
    string Cnpj,
    bool Ativo,
    string? PlanoNome,
    int? StatusAssinatura,
    int UsuariosAtivos,
    DateTime CreatedAtUtc);
