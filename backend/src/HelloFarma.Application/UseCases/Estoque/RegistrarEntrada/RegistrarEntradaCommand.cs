using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarEntrada;

/// <summary>
/// Registra entrada de estoque. Se o lote já existir para o produto, soma a quantidade;
/// caso contrário, cria um novo lote.
/// </summary>
public record RegistrarEntradaCommand(
    Guid ProdutoId,
    string NumeroLote,
    DateOnly Validade,
    int Quantidade,
    string? Localizacao,
    string? Motivo) : IRequest<Guid>;
