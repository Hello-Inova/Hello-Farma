using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.AtivarTenant;

public record AtivarTenantCommand(Guid TenantId) : IRequest<Unit>;
