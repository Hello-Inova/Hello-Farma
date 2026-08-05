using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Tenants;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class TenantRepository(HelloFarmaDbContext context) : EfRepository<Tenant>(context), ITenantRepository
{
    public async Task<bool> CnpjExisteAsync(string cnpj, CancellationToken ct = default) =>
        await context.Tenants.IgnoreQueryFilters().AnyAsync(t => t.Cnpj == cnpj && !t.IsDeleted, ct);
}
