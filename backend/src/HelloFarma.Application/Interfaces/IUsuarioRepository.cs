using HelloFarma.Domain.Entities.Usuarios;

namespace HelloFarma.Application.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);

    /// <summary>Conta usuários ativos por tenant, em toda a plataforma (ignora o filtro de tenant).</summary>
    Task<IReadOnlyDictionary<Guid, int>> ContarAtivosPorTenantAsync(CancellationToken ct = default);
}
