using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Crm;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class ClienteRepository(HelloFarmaDbContext context) : EfRepository<Cliente>(context), IClienteRepository
{
    public async Task<IReadOnlyList<Cliente>> BuscarAsync(string termo, CancellationToken ct = default) =>
        await context.Clientes
            .Where(c => !c.IsDeleted && (c.Nome.Contains(termo) || (c.Cpf != null && c.Cpf.Contains(termo))))
            .ToListAsync(ct);
}
