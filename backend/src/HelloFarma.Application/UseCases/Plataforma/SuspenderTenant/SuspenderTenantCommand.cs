using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.SuspenderTenant;

public record SuspenderTenantCommand(Guid TenantId) : IRequest<Unit>;
