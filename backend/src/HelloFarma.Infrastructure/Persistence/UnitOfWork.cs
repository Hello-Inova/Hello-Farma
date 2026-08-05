using HelloFarma.Application.Interfaces;

namespace HelloFarma.Infrastructure.Persistence;

public class UnitOfWork(HelloFarmaDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
}
