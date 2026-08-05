using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Vendas;

/// <summary>
/// Troca/devolução (total ou parcial) de uma Venda do PDV. Reestabelece o estoque dos
/// itens devolvidos, gera um lançamento financeiro de saída (dinheiro devolvido ao
/// cliente) e, se a venda tinha cliente vinculado, estorna proporcionalmente o cashback
/// gerado por ela.
/// </summary>
public class Devolucao : BaseEntity
{
    public Guid VendaId { get; private set; }
    public string? Motivo { get; private set; }
    public decimal ValorTotal { get; private set; }
    public decimal CashbackEstornado { get; private set; }
    public DateTime RealizadaEmUtc { get; private set; } = DateTime.UtcNow;

    private readonly List<ItemDevolucao> _itens = new();
    public IReadOnlyCollection<ItemDevolucao> Itens => _itens.AsReadOnly();

    protected Devolucao() { }

    public Devolucao(Guid tenantId, Guid vendaId, string? motivo)
    {
        TenantId = tenantId;
        VendaId = vendaId;
        Motivo = motivo;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser positiva.");

        var item = new ItemDevolucao(TenantId, Id, produtoId, produtoNome, quantidade, precoUnitario);
        _itens.Add(item);
        ValorTotal += item.Subtotal;
    }

    public void DefinirCashbackEstornado(decimal valor) => CashbackEstornado = valor;
}
