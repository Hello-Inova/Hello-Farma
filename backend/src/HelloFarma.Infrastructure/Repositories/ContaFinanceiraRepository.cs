using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Enums;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class ContaFinanceiraRepository(HelloFarmaDbContext context) : EfRepository<ContaFinanceira>(context), IContaFinanceiraRepository
{
    public async Task<IReadOnlyList<ContaFinanceira>> ListarAsync(TipoContaFinanceira? tipo, StatusContaFinanceira? status, CancellationToken ct = default)
    {
        var query = context.ContasFinanceiras.Where(c => !c.IsDeleted).AsQueryable();
        if (tipo.HasValue) query = query.Where(c => c.Tipo == tipo.Value);
        if (status.HasValue) query = query.Where(c => c.Status == status.Value);
        return await query.OrderBy(c => c.DataVencimento).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<ContaFinanceira>> ListarPagasNoPeriodoAsync(DateTime inicioUtc, DateTime fimUtc, CancellationToken ct = default) =>
        await context.ContasFinanceiras
            .Where(c => !c.IsDeleted && c.Status == StatusContaFinanceira.Paga && c.PagaEmUtc >= inicioUtc && c.PagaEmUtc < fimUtc)
            .ToListAsync(ct);
}
