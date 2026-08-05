namespace HelloFarma.Application.DTOs;

public record LoteDto(
    Guid Id,
    Guid ProdutoId,
    string ProdutoNome,
    Guid? FilialId,
    string NumeroLote,
    DateOnly Validade,
    int QuantidadeAtual,
    string? Localizacao,
    int DiasParaVencer);
