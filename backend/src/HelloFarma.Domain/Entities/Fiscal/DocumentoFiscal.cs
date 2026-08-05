using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Fiscal;

/// <summary>
/// Representa o documento fiscal (NFC-e/NF-e) vinculado a uma venda. A integração real
/// com SEFAZ é um adaptador de infraestrutura plugável (Strategy) — esta entidade já
/// prepara o domínio para isso sem acoplar o restante do sistema aos detalhes fiscais.
/// </summary>
public class DocumentoFiscal : BaseEntity
{
    public Guid VendaId { get; private set; }
    public StatusDocumentoFiscal Status { get; private set; } = StatusDocumentoFiscal.Pendente;
    public string? ChaveAcesso { get; private set; }
    public string? MotivoRejeicao { get; private set; }
    public DateTime? EmitidoEmUtc { get; private set; }

    protected DocumentoFiscal() { }

    public DocumentoFiscal(Guid tenantId, Guid vendaId)
    {
        TenantId = tenantId;
        VendaId = vendaId;
    }

    public void MarcarEmitido(string chaveAcesso)
    {
        Status = StatusDocumentoFiscal.Emitido;
        ChaveAcesso = chaveAcesso;
        EmitidoEmUtc = DateTime.UtcNow;
        Touch();
    }

    public void MarcarRejeitado(string motivo)
    {
        Status = StatusDocumentoFiscal.Rejeitado;
        MotivoRejeicao = motivo;
        Touch();
    }
}
