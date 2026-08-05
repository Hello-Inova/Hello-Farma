using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.AtivarTenant;

public class AtivarTenantHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AtivarTenantCommand, Unit>
{
    public async Task<Unit> Handle(AtivarTenantCommand request, CancellationToken ct)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, ct)
            ?? throw new KeyNotFoundException("Farmácia não encontrada.");

        tenant.Ativar();
        tenantRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
