using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Billing;

/// <summary>
/// Assinatura de uma farmácia (Tenant) a um Plano. Fluxo de pagamento Farmácia → Hello Inova,
/// distinto do fluxo de venda Cliente → Farmácia.
/// </summary>
public class Assinatura : BaseEntity
{
    public Guid PlanoId { get; private set; }
    public StatusAssinatura Status { get; private set; } = StatusAssinatura.Trial;
    public DateOnly InicioEm { get; private set; }
    public DateOnly? FimEm { get; private set; }

    protected Assinatura() { }

    public Assinatura(Guid tenantId, Guid planoId)
    {
        TenantId = tenantId;
        PlanoId = planoId;
        InicioEm = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public void Ativar() => Status = StatusAssinatura.Ativa;

    public void Cancelar()
    {
        Status = StatusAssinatura.Cancelada;
        FimEm = DateOnly.FromDateTime(DateTime.UtcNow);
        Touch();
    }

    public void MarcarInadimplente() => Status = StatusAssinatura.Inadimplente;
}
