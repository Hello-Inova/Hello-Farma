using MediatR;

namespace HelloFarma.Application.UseCases.Billing.CriarAssinatura;

public record CriarAssinaturaCommand(Guid PlanoId) : IRequest<Guid>;
