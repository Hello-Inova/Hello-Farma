using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Crm;
using MediatR;

namespace HelloFarma.Application.UseCases.Crm.CriarCliente;

public class CriarClienteHandler(IClienteRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarClienteCommand, Guid>
{
    public async Task<Guid> Handle(CriarClienteCommand request, CancellationToken ct)
    {
        var cliente = new Cliente(currentTenant.TenantId, request.Nome, request.Cpf, request.Telefone, request.Email);
        await repository.AddAsync(cliente, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return cliente.Id;
    }
}
