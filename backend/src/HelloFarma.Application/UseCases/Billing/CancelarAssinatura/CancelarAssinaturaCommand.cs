using MediatR;

namespace HelloFarma.Application.UseCases.Billing.CancelarAssinatura;

public record CancelarAssinaturaCommand(Guid AssinaturaId) : IRequest<Unit>;
