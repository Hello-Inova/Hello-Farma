using HelloFarma.Domain.Entities.Usuarios;

namespace HelloFarma.Application.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);
}
