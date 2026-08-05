using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.DesativarFilial;

public class DesativarFilialHandler(IFilialRepository filialRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DesativarFilialCommand, Unit>
{
    public async Task<Unit> Handle(DesativarFilialCommand request, CancellationToken ct)
    {
        var filial = await filialRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Filial não encontrada.");

        if (filial.Matriz)
            throw new InvalidOperationException("A filial matriz não pode ser desativada.");

        filial.Desativar();
        filialRepository.Update(filial);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
