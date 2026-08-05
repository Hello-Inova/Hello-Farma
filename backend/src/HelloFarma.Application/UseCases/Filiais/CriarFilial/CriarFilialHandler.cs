using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Empresa;
using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.CriarFilial;

public class CriarFilialHandler(
    IFilialRepository filialRepository,
    IAssinaturaRepository assinaturaRepository,
    IPlanoRepository planoRepository,
    ICurrentTenant currentTenant,
    IUnitOfWork unitOfWork) : IRequestHandler<CriarFilialCommand, Guid>
{
    public async Task<Guid> Handle(CriarFilialCommand request, CancellationToken ct)
    {
        var assinatura = await assinaturaRepository.ObterAtivaDoTenantAsync(ct);
        if (assinatura is not null)
        {
            var plano = await planoRepository.GetByIdAsync(assinatura.PlanoId, ct);
            if (plano is not null)
            {
                var filiaisAtivas = await filialRepository.ContarAtivasAsync(ct);
                if (filiaisAtivas >= plano.LimiteFiliais)
                    throw new InvalidOperationException(
                        $"Limite de {plano.LimiteFiliais} filial(is) do plano '{plano.Nome}' atingido. Faça upgrade do plano para cadastrar mais filiais.");
            }
        }

        var filial = new Filial(currentTenant.TenantId, request.Nome, request.Cnpj, request.Endereco, request.Cidade, request.Uf);

        await filialRepository.AddAsync(filial, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return filial.Id;
    }
}
