using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Billing;
using HelloFarma.Domain.Enums;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class AssinaturaRepository(HelloFarmaDbContext context) : EfRepository<Assinatura>(context), IAssinaturaRepository
{
    public async Task<Assinatura?> ObterAtivaDoTenantAsync(CancellationToken ct = default) =>
        await context.Assinaturas
            .Where(a => !a.IsDeleted && (a.Status == StatusAssinatura.Ativa || a.Status == StatusAssinatura.Trial))
            .OrderByDescending(a => a.InicioEm)
            .FirstOrDefaultAsync(ct);
}
