using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Common;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class EfRepository<T>(HelloFarmaDbContext context) : IRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct);

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default) =>
        await context.Set<T>().Where(e => !e.IsDeleted).ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await context.Set<T>().AddAsync(entity, ct);

    public void Update(T entity) => context.Set<T>().Update(entity);

    public void Remove(T entity) => entity.MarkAsDeleted();
}
