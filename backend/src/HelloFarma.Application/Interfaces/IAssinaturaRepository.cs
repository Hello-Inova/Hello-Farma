using HelloFarma.Domain.Entities.Billing;

namespace HelloFarma.Application.Interfaces;

public interface IAssinaturaRepository : IRepository<Assinatura>
{
    Task<Assinatura?> ObterAtivaDoTenantAsync(CancellationToken ct = default);
}
