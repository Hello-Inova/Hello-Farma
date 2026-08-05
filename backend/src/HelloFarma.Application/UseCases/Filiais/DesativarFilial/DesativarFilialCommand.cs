using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.DesativarFilial;

public record DesativarFilialCommand(Guid Id) : IRequest<Unit>;
