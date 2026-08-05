using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Auditoria;

/// <summary>
/// Registro de auditoria: quem fez o quê, quando, e se teve sucesso. Gerado automaticamente
/// para toda ação de escrita (Command) executada por um usuário autenticado — nunca editável
/// nem removível pela aplicação, apenas consultável.
/// </summary>
public class LogAuditoria : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public string? UsuarioNome { get; private set; }
    public string Acao { get; private set; } = default!;
    public bool Sucesso { get; private set; }
    public string? Erro { get; private set; }
    public string? IpAddress { get; private set; }

    protected LogAuditoria() { }

    public LogAuditoria(Guid tenantId, Guid usuarioId, string? usuarioNome, string acao, bool sucesso, string? erro, string? ipAddress)
    {
        TenantId = tenantId;
        UsuarioId = usuarioId;
        UsuarioNome = usuarioNome;
        Acao = acao;
        Sucesso = sucesso;
        Erro = erro;
        IpAddress = ipAddress;
    }
}
