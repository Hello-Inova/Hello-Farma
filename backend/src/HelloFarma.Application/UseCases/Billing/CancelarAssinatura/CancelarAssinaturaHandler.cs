using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Billing.CancelarAssinatura;

public class CancelarAssinaturaHandler(IAssinaturaRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CancelarAssinaturaCommand, Unit>
{
    public async Task<Unit> Handle(CancelarAssinaturaCommand request, CancellationToken ct)
    {
        var assinatura = await repository.GetByIdAsync(request.AssinaturaId, ct) ?? throw new KeyNotFoundException("Assinatura não encontrada.");
        assinatura.Cancelar();
        repository.Update(assinatura);
        await unitOfWork.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
