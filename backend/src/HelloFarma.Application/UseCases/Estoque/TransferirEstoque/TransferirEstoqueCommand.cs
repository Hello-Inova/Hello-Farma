using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.TransferirEstoque;

/// <summary>
/// Transfere quantidade de um lote entre duas filiais: baixa na filial de origem
/// e entra (soma ou cria lote) na filial de destino, mantendo o mesmo número de lote
/// e validade para rastreabilidade. Registra movimentação de Saída na origem e
/// Entrada no destino, ambas com o motivo "Transferência entre filiais".
/// </summary>
public record TransferirEstoqueCommand(
    Guid ProdutoId,
    string NumeroLote,
    DateOnly Validade,
    int Quantidade,
    Guid FilialOrigemId,
    Guid FilialDestinoId) : IRequest<Unit>;
