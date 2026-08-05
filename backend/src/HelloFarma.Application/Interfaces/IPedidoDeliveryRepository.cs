using HelloFarma.Domain.Entities.Delivery;

namespace HelloFarma.Application.Interfaces;

public interface IPedidoDeliveryRepository : IRepository<PedidoDelivery>
{
    Task<IReadOnlyList<PedidoDelivery>> ListarEmAndamentoAsync(CancellationToken ct = default);
}
