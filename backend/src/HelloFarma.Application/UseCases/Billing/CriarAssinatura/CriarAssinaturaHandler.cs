using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Billing;
using MediatR;

namespace HelloFarma.Application.UseCases.Billing.CriarAssinatura;

/// <summary>Inicia a assinatura da farmácia em um plano (fluxo de pagamento Farmácia → Hello Inova).</summary>
public class CriarAssinaturaHandler(IAssinaturaRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarAssinaturaCommand, Guid>
{
    public async Task<Guid> Handle(CriarAssinaturaCommand request, CancellationToken ct)
    {
        var assinatura = new Assinatura(currentTenant.TenantId, request.PlanoId);
        await repository.AddAsync(assinatura, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return assinatura.Id;
    }
}
