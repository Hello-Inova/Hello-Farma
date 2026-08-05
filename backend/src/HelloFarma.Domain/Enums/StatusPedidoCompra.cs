namespace HelloFarma.Domain.Enums;

/// <summary>
/// Fluxo do módulo Compras: Cotação → Pedido → Recebimento → Conferência → Entrada.
/// </summary>
public enum StatusPedidoCompra
{
    Cotacao = 1,
    PedidoRealizado = 2,
    Recebido = 3,
    Conferido = 4,
    Cancelado = 9
}
