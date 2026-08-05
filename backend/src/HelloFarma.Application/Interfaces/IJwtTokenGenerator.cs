using HelloFarma.Domain.Entities.Usuarios;

namespace HelloFarma.Application.Interfaces;

public record AccessTokenResult(string Token, DateTime ExpiraEmUtc);

public interface IJwtTokenGenerator
{
    AccessTokenResult GerarAccessToken(Usuario usuario);
    string GerarRefreshToken();
}
