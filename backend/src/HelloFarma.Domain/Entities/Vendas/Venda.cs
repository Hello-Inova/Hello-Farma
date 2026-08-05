using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Vendas;

/// <summary>
/// Venda realizada no PDV. Toda venda finalizada possui pagamento vinculado
/// (nunca uma venda sem forma de pagamento definida), conforme diretriz do Hello Farma.
/// Pode ser atribuída a uma Filial específica e a um Cliente (habilitando cashback e
/// fidelização), quando o tenant opera com essas funcionalidades.
/// </summary>
public class Venda : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public Guid? ClienteId { get; private set; }
    public Guid? FilialId { get; private set; }
    public FormaPagamento FormaPagamento { get; private set; }
    public StatusVenda Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public decimal CashbackUtilizado { get; private set; }
    public decimal CashbackGerado { get; private set; }
    public decimal ValorPago => ValorTotal - CashbackUtilizado;
    public DateTime RealizadaEmUtc { get; private set; } = DateTime.UtcNow;

    private readonly List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();

    protected Venda() { }

    public Venda(Guid tenantId, Guid usuarioId, FormaPagamento formaPagamento, Guid? clienteId = null, Guid? filialId = null)
    {
        TenantId = tenantId;
        UsuarioId = usuarioId;
        FormaPagamento = formaPagamento;
        ClienteId = clienteId;
        FilialId = filialId;
        Status = StatusVenda.Finalizada;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser positiva.");

        var item = new ItemVenda(TenantId, Id, produtoId, produtoNome, quantidade, precoUnitario);
        _itens.Add(item);
        ValorTotal += item.Subtotal;
    }

    /// <summary>
    /// Aplica o cashback resgatado (usado como parte do pagamento) e o cashback gerado
    /// por esta venda (creditado ao cliente). Deve ser chamado uma única vez, após todos
    /// os itens terem sido adicionados.
    /// </summary>
    public void AplicarCashback(decimal cashbackUtilizado, decimal cashbackGerado)
    {
        if (cashbackUtilizado < 0 || cashbackGerado < 0)
            throw new InvalidOperationException("Valores de cashback não podem ser negativos.");
        if (cashbackUtilizado > ValorTotal)
            throw new InvalidOperationException("O cashback utilizado não pode ser maior que o total da venda.");

        CashbackUtilizado = cashbackUtilizado;
        CashbackGerado = cashbackGerado;
    }

    public void Cancelar() => Status = StatusVenda.Cancelada;

    public void MarcarParcialmenteDevolvida()
    {
        if (Status is StatusVenda.Finalizada or StatusVenda.ParcialmenteDevolvida)
            Status = StatusVenda.ParcialmenteDevolvida;
    }

    public void MarcarDevolvida() => Status = StatusVenda.Devolvida;
}
