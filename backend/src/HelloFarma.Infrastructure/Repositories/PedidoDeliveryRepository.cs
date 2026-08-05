using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Delivery;
using HelloFarma.Domain.Enums;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class PedidoDeliveryRepository(HelloFarmaDbContext context) : EfRepository<PedidoDelivery>(context), IPedidoDeliveryRepository
{
    public async Task<IReadOnlyList<PedidoDelivery>> ListarEmAndamentoAsync(CancellationToken ct = default) =>
        await context.PedidosDelivery
            .Where(p => !p.IsDeleted && p.Status != StatusPedidoDelivery.Avaliado && p.Status != StatusPedidoDelivery.Cancelado)
            .ToListAsync(ct);
}
