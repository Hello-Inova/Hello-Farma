using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.BaixarConta;

public record BaixarContaCommand(Guid Id) : IRequest<Unit>;
