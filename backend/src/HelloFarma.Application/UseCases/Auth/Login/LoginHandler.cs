using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Usuarios;
using MediatR;
using DomainRefreshToken = HelloFarma.Domain.Entities.Auth.RefreshToken;

namespace HelloFarma.Application.UseCases.Auth.Login;

/// <summary>
/// Caso de uso: autenticar um usuário (validação de senha, geração de access + refresh token).
/// Regra de negócio: usuário deve estar ativo e a farmácia (tenant) também deve estar ativa.
/// </summary>
public class LoginHandler(
    IUsuarioRepository usuarioRepository,
    ITenantRepository tenantRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IRepository<DomainRefreshToken> refreshTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var usuario = await usuarioRepository.ObterPorEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário inativo.");

        if (!passwordHasher.Verificar(request.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var tenant = await tenantRepository.GetByIdAsync(usuario.TenantId, ct)
            ?? throw new UnauthorizedAccessException("Farmácia não encontrada.");

        if (!tenant.Ativo)
            throw new UnauthorizedAccessException("Farmácia inativa. Entre em contato com o suporte.");

        var accessToken = jwtTokenGenerator.GerarAccessToken(usuario);
        var refreshTokenValue = jwtTokenGenerator.GerarRefreshToken();

        var refreshToken = new DomainRefreshToken(usuario.TenantId, usuario.Id, refreshTokenValue, DateTime.UtcNow.AddDays(30));
        await refreshTokenRepository.AddAsync(refreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new LoginResult(
            accessToken.Token,
            accessToken.ExpiraEmUtc,
            refreshTokenValue,
            usuario.Nome,
            usuario.Papel.ToString(),
            usuario.TenantId);
    }
}
