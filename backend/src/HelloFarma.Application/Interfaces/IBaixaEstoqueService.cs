namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Serviço de domínio/aplicação compartilhado para baixa de estoque seguindo a regra FEFO.
/// Usado tanto pela baixa manual (módulo Estoque) quanto pela venda (PDV), evitando duplicação
/// da regra de negócio (Service Pattern). Quando filialId é informado, a baixa FEFO considera
/// apenas os lotes daquela filial.
/// </summary>
public interface IBaixaEstoqueService
{
    Task BaixarAsync(Guid produtoId, int quantidade, string motivo, Guid? filialId = null, CancellationToken ct = default);
}
