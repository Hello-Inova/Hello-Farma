using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.AtivarFilial;

public record AtivarFilialCommand(Guid Id) : IRequest<Unit>;
