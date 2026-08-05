using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.BaixarConta;

public class BaixarContaHandler(IContaFinanceiraRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<BaixarContaCommand, Unit>
{
    public async Task<Unit> Handle(BaixarContaCommand request, CancellationToken ct)
    {
        var conta = await repository.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Conta não encontrada.");
        conta.MarcarComoPaga();
        repository.Update(conta);
        await unitOfWork.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
