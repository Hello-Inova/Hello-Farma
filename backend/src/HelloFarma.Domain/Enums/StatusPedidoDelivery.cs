namespace HelloFarma.Domain.Enums;

/// <summary>
/// Fluxo do módulo Delivery: Pedido → Pagamento → Separação → Expedição → Entrega → Avaliação.
/// </summary>
public enum StatusPedidoDelivery
{
    Pendente = 1,
    Separacao = 2,
    Expedicao = 3,
    EmRota = 4,
    Entregue = 5,
    Avaliado = 6,
    Cancelado = 9
}
