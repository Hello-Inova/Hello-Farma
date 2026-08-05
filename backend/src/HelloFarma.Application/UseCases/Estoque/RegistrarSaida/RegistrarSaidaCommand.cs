using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarSaida;

public record RegistrarSaidaCommand(Guid ProdutoId, int Quantidade, string? Motivo, Guid? FilialId = null) : IRequest<Unit>;
