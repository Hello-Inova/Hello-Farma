using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Compras;

public class ItemPedidoCompra : BaseEntity
{
    public Guid PedidoCompraId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; } = default!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public string NumeroLote { get; private set; } = default!;
    public DateOnly Validade { get; private set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;

    protected ItemPedidoCompra() { }

    public ItemPedidoCompra(Guid tenantId, Guid pedidoCompraId, Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario, string numeroLote, DateOnly validade)
    {
        TenantId = tenantId;
        PedidoCompraId = pedidoCompraId;
        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        NumeroLote = numeroLote;
        Validade = validade;
    }
}
