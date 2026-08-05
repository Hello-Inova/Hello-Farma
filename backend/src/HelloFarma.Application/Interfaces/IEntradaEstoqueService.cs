namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Serviço compartilhado de entrada de estoque (Service Pattern), usado tanto pelo
/// módulo Estoque (entrada manual/avulsa) quanto pelo módulo Compras (recebimento de pedido).
/// Quando filialId é informado, o lote é vinculado à filial correspondente.
/// </summary>
public interface IEntradaEstoqueService
{
    Task<Guid> EntrarAsync(Guid produtoId, string numeroLote, DateOnly validade, int quantidade, string? localizacao, string motivo, Guid? filialId = null, CancellationToken ct = default);
}
