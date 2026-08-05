using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Auditoria;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class AuditoriaRepository(HelloFarmaDbContext context) : EfRepository<LogAuditoria>(context), IAuditoriaRepository
{
    public async Task<IReadOnlyList<LogAuditoria>> ListarRecentesAsync(int quantidade, CancellationToken ct = default) =>
        await context.Set<LogAuditoria>()
            .OrderByDescending(l => l.CreatedAtUtc)
            .Take(quantidade)
            .ToListAsync(ct);
}
