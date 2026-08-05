using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Usuarios;

/// <summary>
/// Usuário do sistema, sempre vinculado a um Tenant (farmácia).
/// Nunca compartilhado entre farmácias diferentes.
/// </summary>
public class Usuario : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string SenhaHash { get; private set; } = default!;
    public PapelUsuario Papel { get; private set; }
    public bool Ativo { get; private set; } = true;

    protected Usuario() { }

    public Usuario(Guid tenantId, string nome, string email, string senhaHash, PapelUsuario papel)
    {
        TenantId = tenantId;
        Nome = nome;
        Email = email.Trim().ToLowerInvariant();
        SenhaHash = senhaHash;
        Papel = papel;
    }

    public void AtualizarSenha(string novaSenhaHash)
    {
        SenhaHash = novaSenhaHash;
        Touch();
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
