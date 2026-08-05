using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Vendas;

/// <summary>
/// Venda realizada no PDV. Toda venda finalizada possui pagamento vinculado
/// (nunca uma venda sem forma de pagamento definida), conforme diretriz do Hello Farma.
/// </summary>
public class Venda : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public Guid? ClienteId { get; private set; }
    public FormaPagamento FormaPagamento { get; private set; }
    public StatusVenda Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime RealizadaEmUtc { get; private set; } = DateTime.UtcNow;

    private readonly List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();

    protected Venda() { }

    public Venda(Guid tenantId, Guid usuarioId, FormaPagamento formaPagamento, Guid? clienteId = null)
    {
        TenantId = tenantId;
        UsuarioId = usuarioId;
        FormaPagamento = formaPagamento;
        ClienteId = clienteId;
        Status = StatusVenda.Finalizada;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser positiva.");

        var item = new ItemVenda(TenantId, Id, produtoId, produtoNome, quantidade, precoUnitario);
        _itens.Add(item);
        ValorTotal += item.Subtotal;
    }

    public void Cancelar() => Status = StatusVenda.Cancelada;
}
