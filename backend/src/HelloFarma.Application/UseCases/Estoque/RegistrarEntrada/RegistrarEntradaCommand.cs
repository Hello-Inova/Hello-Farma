using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarEntrada;

/// <summary>
/// Registra entrada de estoque. Se o lote já existir para o produto (na mesma filial),
/// soma a quantidade; caso contrário, cria um novo lote.
/// </summary>
public record RegistrarEntradaCommand(
    Guid ProdutoId,
    string NumeroLote,
    DateOnly Validade,
    int Quantidade,
    string? Localizacao,
    string? Motivo,
    Guid? FilialId = null) : IRequest<Guid>;
