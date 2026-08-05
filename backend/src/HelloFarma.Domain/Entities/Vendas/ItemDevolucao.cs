using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Vendas;

public class ItemDevolucao : BaseEntity
{
    public Guid DevolucaoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; } = default!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;

    protected ItemDevolucao() { }

    public ItemDevolucao(Guid tenantId, Guid devolucaoId, Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        TenantId = tenantId;
        DevolucaoId = devolucaoId;
        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}
