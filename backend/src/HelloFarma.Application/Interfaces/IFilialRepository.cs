using HelloFarma.Domain.Entities.Empresa;

namespace HelloFarma.Application.Interfaces;

public interface IFilialRepository : IRepository<Filial>
{
    Task<int> ContarAtivasAsync(CancellationToken ct = default);
}
