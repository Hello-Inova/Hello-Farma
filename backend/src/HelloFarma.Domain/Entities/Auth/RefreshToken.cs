using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Auth;

/// <summary>
/// Refresh token de longa duração usado para renovar o JWT de acesso sem exigir novo login.
/// </summary>
public class RefreshToken : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public string Token { get; private set; } = default!;
    public DateTime ExpiraEmUtc { get; private set; }
    public bool Revogado { get; private set; }

    protected RefreshToken() { }

    public RefreshToken(Guid tenantId, Guid usuarioId, string token, DateTime expiraEmUtc)
    {
        TenantId = tenantId;
        UsuarioId = usuarioId;
        Token = token;
        ExpiraEmUtc = expiraEmUtc;
    }

    public bool EstaValido() => !Revogado && ExpiraEmUtc > DateTime.UtcNow;

    public void Revogar() => Revogado = true;
}
