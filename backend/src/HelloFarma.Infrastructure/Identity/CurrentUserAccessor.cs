using System.Security.Claims;
using HelloFarma.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HelloFarma.Infrastructure.Identity;

public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid UsuarioId
    {
        get
        {
            var claim = User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }

    public string? Nome => User?.FindFirst(ClaimTypes.Name)?.Value;
    public string? Papel => User?.FindFirst(ClaimTypes.Role)?.Value;
}
