using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Usuarios;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class UsuarioRepository(HelloFarmaDbContext context) : EfRepository<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default) =>
        await context.Usuarios
            .IgnoreQueryFilters() // login precisa localizar o usuário antes de saber o tenant
            .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower() && !u.IsDeleted, ct);

    public async Task<IReadOnlyDictionary<Guid, int>> ContarAtivosPorTenantAsync(CancellationToken ct = default)
    {
        var contagens = await context.Usuarios
            .IgnoreQueryFilters()
            .Where(u => !u.IsDeleted && u.Ativo)
            .GroupBy(u => u.TenantId)
            .Select(g => new { TenantId = g.Key, Total = g.Count() })
            .ToListAsync(ct);

        return contagens.ToDictionary(c => c.TenantId, c => c.Total);
    }
}
