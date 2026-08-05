using MediatR;

namespace HelloFarma.Application.UseCases.Auth.RegistrarTenant;

/// <summary>
/// Cadastra uma nova farmácia (Tenant) na plataforma junto com seu usuário administrador inicial.
/// </summary>
public record RegistrarTenantCommand(
    string RazaoSocial,
    string NomeFantasia,
    string Cnpj,
    string PlanoId,
    string NomeAdmin,
    string EmailAdmin,
    string SenhaAdmin) : IRequest<Guid>;
