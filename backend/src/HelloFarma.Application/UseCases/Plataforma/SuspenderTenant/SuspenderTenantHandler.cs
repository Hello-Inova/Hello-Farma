using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.SuspenderTenant;

public class SuspenderTenantHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<SuspenderTenantCommand, Unit>
{
    public async Task<Unit> Handle(SuspenderTenantCommand request, CancellationToken ct)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, ct)
            ?? throw new KeyNotFoundException("Farmácia não encontrada.");

        tenant.Desativar();
        tenantRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
