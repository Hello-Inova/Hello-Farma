using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.AtivarFilial;

public class AtivarFilialHandler(IFilialRepository filialRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AtivarFilialCommand, Unit>
{
    public async Task<Unit> Handle(AtivarFilialCommand request, CancellationToken ct)
    {
        var filial = await filialRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Filial não encontrada.");

        filial.Ativar();
        filialRepository.Update(filial);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
