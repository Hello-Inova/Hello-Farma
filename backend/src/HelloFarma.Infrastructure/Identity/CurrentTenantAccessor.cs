using HelloFarma.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HelloFarma.Infrastructure.Identity;

/// <summary>
/// Resolve o Tenant (farmácia) do usuário autenticado a partir da claim "tenant_id" do JWT.
/// É a peça central do isolamento multi-empresa: toda consulta ao banco passa por aqui.
/// </summary>
public class CurrentTenantAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentTenant
{
    public Guid TenantId
    {
        get
        {
            var claim = httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value;
            return Guid.TryParse(claim, out var tenantId) ? tenantId : Guid.Empty;
        }
    }
}
