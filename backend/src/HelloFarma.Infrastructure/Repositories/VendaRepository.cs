using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Vendas;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class VendaRepository(HelloFarmaDbContext context) : EfRepository<Venda>(context), IVendaRepository
{
    public async Task<IReadOnlyList<Venda>> ListarPorPeriodoAsync(DateTime inicioUtc, DateTime fimUtc, CancellationToken ct = default) =>
        await context.Vendas
            .Include(v => v.Itens)
            .Where(v => !v.IsDeleted && v.RealizadaEmUtc >= inicioUtc && v.RealizadaEmUtc < fimUtc)
            .OrderByDescending(v => v.RealizadaEmUtc)
            .ToListAsync(ct);
}
