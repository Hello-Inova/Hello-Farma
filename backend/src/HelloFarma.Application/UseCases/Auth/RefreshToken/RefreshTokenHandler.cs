using HelloFarma.Application.Interfaces;
using MediatR;
using DomainRefreshToken = HelloFarma.Domain.Entities.Auth.RefreshToken;

namespace HelloFarma.Application.UseCases.Auth.RefreshToken;

/// <summary>
/// Renova o access token a partir de um refresh token válido (rotação de token:
/// o token usado é revogado e um novo é emitido).
/// </summary>
public class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUsuarioRepository usuarioRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var tokenAtual = await refreshTokenRepository.ObterPorTokenAsync(request.RefreshToken, ct)
            ?? throw new UnauthorizedAccessException("Refresh token inválido.");

        if (!tokenAtual.EstaValido())
            throw new UnauthorizedAccessException("Refresh token expirado ou revogado.");

        var usuario = await usuarioRepository.GetByIdAsync(tokenAtual.UsuarioId, ct)
            ?? throw new UnauthorizedAccessException("Usuário não encontrado.");

        tokenAtual.Revogar();

        var novoAccessToken = jwtTokenGenerator.GerarAccessToken(usuario);
        var novoRefreshTokenValue = jwtTokenGenerator.GerarRefreshToken();
        var novoRefreshToken = new DomainRefreshToken(usuario.TenantId, usuario.Id, novoRefreshTokenValue, DateTime.UtcNow.AddDays(30));

        await refreshTokenRepository.AddAsync(novoRefreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new RefreshTokenResult(novoAccessToken.Token, novoAccessToken.ExpiraEmUtc, novoRefreshTokenValue);
    }
}
