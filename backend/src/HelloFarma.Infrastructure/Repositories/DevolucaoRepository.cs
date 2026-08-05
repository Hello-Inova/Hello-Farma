using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Vendas;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class DevolucaoRepository(HelloFarmaDbContext context) : EfRepository<Devolucao>(context), IDevolucaoRepository
{
    public async Task<IReadOnlyList<Devolucao>> ListarPorVendaAsync(Guid vendaId, CancellationToken ct = default) =>
        await context.Set<Devolucao>()
            .Include(d => d.Itens)
            .Where(d => d.VendaId == vendaId && !d.IsDeleted)
            .ToListAsync(ct);
}
