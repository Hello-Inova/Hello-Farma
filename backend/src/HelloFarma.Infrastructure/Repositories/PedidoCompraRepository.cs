using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Compras;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class PedidoCompraRepository(HelloFarmaDbContext context) : EfRepository<PedidoCompra>(context), IPedidoCompraRepository
{
    public async Task<IReadOnlyList<PedidoCompra>> ListarAsync(CancellationToken ct = default) =>
        await context.PedidosCompra.Include(p => p.Itens).Where(p => !p.IsDeleted).OrderByDescending(p => p.CreatedAtUtc).ToListAsync(ct);
}
