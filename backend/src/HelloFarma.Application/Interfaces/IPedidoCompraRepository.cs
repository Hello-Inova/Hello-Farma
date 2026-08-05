using HelloFarma.Domain.Entities.Compras;

namespace HelloFarma.Application.Interfaces;

public interface IPedidoCompraRepository : IRepository<PedidoCompra>
{
    Task<IReadOnlyList<PedidoCompra>> ListarAsync(CancellationToken ct = default);
}
