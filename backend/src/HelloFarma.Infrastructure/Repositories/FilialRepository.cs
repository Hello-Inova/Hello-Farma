using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Empresa;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class FilialRepository(HelloFarmaDbContext context) : EfRepository<Filial>(context), IFilialRepository
{
    public async Task<int> ContarAtivasAsync(CancellationToken ct = default) =>
        await context.Set<Filial>().CountAsync(f => !f.IsDeleted && f.Ativa, ct);
}
