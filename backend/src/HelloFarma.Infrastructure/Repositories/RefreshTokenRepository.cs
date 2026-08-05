using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Auth;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class RefreshTokenRepository(HelloFarmaDbContext context) : EfRepository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> ObterPorTokenAsync(string token, CancellationToken ct = default) =>
        await context.RefreshTokens
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(rt => rt.Token == token, ct);
}
