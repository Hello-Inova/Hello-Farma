using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Billing.ListarPlanos;

public class ListarPlanosHandler(IPlanoRepository repository) : IRequestHandler<ListarPlanosQuery, IReadOnlyList<PlanoDto>>
{
    public async Task<IReadOnlyList<PlanoDto>> Handle(ListarPlanosQuery request, CancellationToken ct)
    {
        var planos = await repository.ListAsync(ct);
        return planos.Select(p => new PlanoDto(p.Id, p.Nome, p.PrecoMensal, p.LimiteUsuarios, p.LimiteFiliais, p.LimiteProdutos, p.PermiteDelivery, p.PermiteIA)).ToList();
    }
}
