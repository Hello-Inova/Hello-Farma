namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Abstração que expõe o Tenant (farmácia) do usuário autenticado na requisição atual.
/// Usado por toda a camada de aplicação para aplicar o filtro multi-empresa.
/// </summary>
public interface ICurrentTenant
{
    Guid TenantId { get; }
}
