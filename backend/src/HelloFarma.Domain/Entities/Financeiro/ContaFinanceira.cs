using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Financeiro;

/// <summary>
/// Lançamento financeiro único (conta a pagar ou a receber). Alimenta o fluxo de caixa
/// da farmácia. Pode ser criado manualmente ou automaticamente (venda no PDV gera
/// conta a receber já paga; recebimento de pedido de compra gera conta a pagar pendente).
/// </summary>
public class ContaFinanceira : BaseEntity
{
    public TipoContaFinanceira Tipo { get; private set; }
    public string Descricao { get; private set; } = default!;
    public decimal Valor { get; private set; }
    public DateOnly DataVencimento { get; private set; }
    public DateTime? PagaEmUtc { get; private set; }
    public StatusContaFinanceira Status { get; private set; } = StatusContaFinanceira.Pendente;
    public string? OrigemTipo { get; private set; }
    public Guid? OrigemId { get; private set; }

    protected ContaFinanceira() { }

    public ContaFinanceira(Guid tenantId, TipoContaFinanceira tipo, string descricao, decimal valor, DateOnly dataVencimento, string? origemTipo = null, Guid? origemId = null)
    {
        TenantId = tenantId;
        Tipo = tipo;
        Descricao = descricao;
        Valor = valor;
        DataVencimento = dataVencimento;
        OrigemTipo = origemTipo;
        OrigemId = origemId;
    }

    public void MarcarComoPaga()
    {
        if (Status == StatusContaFinanceira.Paga) return;
        Status = StatusContaFinanceira.Paga;
        PagaEmUtc = DateTime.UtcNow;
        Touch();
    }

    public void Cancelar()
    {
        Status = StatusContaFinanceira.Cancelada;
        Touch();
    }

    public static ContaFinanceira CriarJaPaga(Guid tenantId, TipoContaFinanceira tipo, string descricao, decimal valor, string origemTipo, Guid origemId)
    {
        var conta = new ContaFinanceira(tenantId, tipo, descricao, valor, DateOnly.FromDateTime(DateTime.UtcNow), origemTipo, origemId);
        conta.MarcarComoPaga();
        return conta;
    }
}
