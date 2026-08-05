using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.CriarConta;

public class CriarContaHandler(IContaFinanceiraRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarContaCommand, Guid>
{
    public async Task<Guid> Handle(CriarContaCommand request, CancellationToken ct)
    {
        var conta = new ContaFinanceira(currentTenant.TenantId, (TipoContaFinanceira)request.Tipo, request.Descricao, request.Valor, request.DataVencimento);
        await repository.AddAsync(conta, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return conta.Id;
    }
}
