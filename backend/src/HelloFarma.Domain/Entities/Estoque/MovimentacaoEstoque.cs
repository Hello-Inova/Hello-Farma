using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Estoque;

/// <summary>
/// Registro histórico e auditável de toda movimentação de estoque (entrada, saída,
/// ajuste, transferência, perda/avaria) — obrigatório para rastreabilidade e auditoria,
/// especialmente de medicamentos controlados.
/// </summary>
public class MovimentacaoEstoque : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public Guid LoteId { get; private set; }
    public TipoMovimentacaoEstoque Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public string? Motivo { get; private set; }
    public DateTime OcorreuEmUtc { get; private set; } = DateTime.UtcNow;

    protected MovimentacaoEstoque() { }

    public MovimentacaoEstoque(Guid tenantId, Guid produtoId, Guid loteId, TipoMovimentacaoEstoque tipo, int quantidade, string? motivo = null)
    {
        TenantId = tenantId;
        ProdutoId = produtoId;
        LoteId = loteId;
        Tipo = tipo;
        Quantidade = quantidade;
        Motivo = motivo;
    }
}
