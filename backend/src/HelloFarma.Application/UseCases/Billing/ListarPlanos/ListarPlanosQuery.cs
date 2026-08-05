using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Billing.ListarPlanos;

public record ListarPlanosQuery : IRequest<IReadOnlyList<PlanoDto>>;
