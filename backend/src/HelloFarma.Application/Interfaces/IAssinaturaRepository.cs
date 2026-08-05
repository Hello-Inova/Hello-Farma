using HelloFarma.Domain.Entities.Billing;

namespace HelloFarma.Application.Interfaces;

public interface IAssinaturaRepository : IRepository<Assinatura>
{
    Task<Assinatura?> ObterAtivaDoTenantAsync(CancellationToken ct = default);

    /// <summary>Lista a assinatura mais recente de cada tenant da plataforma (ignora o filtro de tenant).</summary>
    Task<IReadOnlyList<Assinatura>> ListarMaisRecentesDeTodosOsTenantsAsync(CancellationToken ct = default);
}
