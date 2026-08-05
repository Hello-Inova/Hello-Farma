using HelloFarma.Domain.Entities.Auth;

namespace HelloFarma.Application.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> ObterPorTokenAsync(string token, CancellationToken ct = default);
}
