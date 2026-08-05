using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Compras;

/// <summary>
/// Pedido de compra junto a um fornecedor. Segue o fluxo obrigatório do Hello Farma:
/// Cotação → Pedido → Recebimento → Conferência → Entrada em estoque.
/// </summary>
public class PedidoCompra : BaseEntity
{
    public Guid FornecedorId { get; private set; }
    public StatusPedidoCompra Status { get; private set; } = StatusPedidoCompra.Cotacao;
    public decimal ValorTotal { get; private set; }
    public DateTime? RecebidoEmUtc { get; private set; }

    private readonly List<ItemPedidoCompra> _itens = new();
    public IReadOnlyCollection<ItemPedidoCompra> Itens => _itens.AsReadOnly();

    protected PedidoCompra() { }

    public PedidoCompra(Guid tenantId, Guid fornecedorId)
    {
        TenantId = tenantId;
        FornecedorId = fornecedorId;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario, string numeroLote, DateOnly validade)
    {
        if (Status != StatusPedidoCompra.Cotacao)
            throw new InvalidOperationException("Só é possível adicionar itens enquanto o pedido está em cotação.");

        var item = new ItemPedidoCompra(TenantId, Id, produtoId, produtoNome, quantidade, precoUnitario, numeroLote, validade);
        _itens.Add(item);
        ValorTotal += item.Subtotal;
    }

    public void ConfirmarPedido()
    {
        if (_itens.Count == 0) throw new InvalidOperationException("O pedido precisa ter ao menos um item.");
        Status = StatusPedidoCompra.PedidoRealizado;
    }

    public void Receber()
    {
        if (Status != StatusPedidoCompra.PedidoRealizado)
            throw new InvalidOperationException("Só é possível receber um pedido que já foi realizado.");

        Status = StatusPedidoCompra.Recebido;
        RecebidoEmUtc = DateTime.UtcNow;
    }

    public void Conferir() => Status = StatusPedidoCompra.Conferido;

    public void Cancelar() => Status = StatusPedidoCompra.Cancelado;
}
