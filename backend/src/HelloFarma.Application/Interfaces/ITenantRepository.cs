using HelloFarma.Domain.Entities.Tenants;

namespace HelloFarma.Application.Interfaces;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<bool> CnpjExisteAsync(string cnpj, CancellationToken ct = default);
}
