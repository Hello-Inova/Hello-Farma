using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Vendas;

public class ItemVenda : BaseEntity
{
    public Guid VendaId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; } = default!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;

    protected ItemVenda() { }

    public ItemVenda(Guid tenantId, Guid vendaId, Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        TenantId = tenantId;
        VendaId = vendaId;
        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}
