using HelloFarma.Domain.Entities.Crm;

namespace HelloFarma.Application.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<IReadOnlyList<Cliente>> BuscarAsync(string termo, CancellationToken ct = default);
}
