using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Compras;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.CriarFornecedor;

public class CriarFornecedorHandler(IFornecedorRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarFornecedorCommand, Guid>
{
    public async Task<Guid> Handle(CriarFornecedorCommand request, CancellationToken ct)
    {
        var fornecedor = new Fornecedor(currentTenant.TenantId, request.RazaoSocial, request.Cnpj, request.Contato, request.Telefone);
        await repository.AddAsync(fornecedor, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return fornecedor.Id;
    }
}
