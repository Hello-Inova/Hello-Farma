using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Delivery;

/// <summary>
/// Pedido de entrega vinculado a uma venda já paga. Acompanhamento em tempo real
/// (via SignalR na API) do status: Pendente → Separação → Expedição → Em rota → Entregue → Avaliado.
/// </summary>
public class PedidoDelivery : BaseEntity
{
    public Guid VendaId { get; private set; }
    public Guid? ClienteId { get; private set; }
    public string EnderecoEntrega { get; private set; } = default!;
    public Guid? EntregadorId { get; private set; }
    public StatusPedidoDelivery Status { get; private set; } = StatusPedidoDelivery.Pendente;
    public int? AvaliacaoNota { get; private set; }
    public DateTime? EntregueEmUtc { get; private set; }

    protected PedidoDelivery() { }

    public PedidoDelivery(Guid tenantId, Guid vendaId, string enderecoEntrega, Guid? clienteId = null)
    {
        TenantId = tenantId;
        VendaId = vendaId;
        EnderecoEntrega = enderecoEntrega;
        ClienteId = clienteId;
    }

    private static readonly Dictionary<StatusPedidoDelivery, StatusPedidoDelivery[]> TransicoesPermitidas = new()
    {
        [StatusPedidoDelivery.Pendente] = [StatusPedidoDelivery.Separacao, StatusPedidoDelivery.Cancelado],
        [StatusPedidoDelivery.Separacao] = [StatusPedidoDelivery.Expedicao, StatusPedidoDelivery.Cancelado],
        [StatusPedidoDelivery.Expedicao] = [StatusPedidoDelivery.EmRota, StatusPedidoDelivery.Cancelado],
        [StatusPedidoDelivery.EmRota] = [StatusPedidoDelivery.Entregue],
        [StatusPedidoDelivery.Entregue] = [StatusPedidoDelivery.Avaliado],
    };

    public void AvancarPara(StatusPedidoDelivery novoStatus)
    {
        if (!TransicoesPermitidas.TryGetValue(Status, out var permitidas) || !permitidas.Contains(novoStatus))
            throw new InvalidOperationException($"Transição inválida de {Status} para {novoStatus}.");

        Status = novoStatus;
        if (novoStatus == StatusPedidoDelivery.Entregue) EntregueEmUtc = DateTime.UtcNow;
        Touch();
    }

    public void AtribuirEntregador(Guid entregadorId)
    {
        EntregadorId = entregadorId;
        Touch();
    }

    public void Avaliar(int nota)
    {
        if (Status != StatusPedidoDelivery.Entregue) throw new InvalidOperationException("Só é possível avaliar um pedido já entregue.");
        if (nota is < 1 or > 5) throw new InvalidOperationException("Nota deve ser entre 1 e 5.");
        AvaliacaoNota = nota;
        Status = StatusPedidoDelivery.Avaliado;
        Touch();
    }
}
