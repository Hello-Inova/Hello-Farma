using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.ListarFiliais;

public class ListarFiliaisHandler(IFilialRepository filialRepository) : IRequestHandler<ListarFiliaisQuery, IReadOnlyList<FilialDto>>
{
    public async Task<IReadOnlyList<FilialDto>> Handle(ListarFiliaisQuery request, CancellationToken ct)
    {
        var filiais = await filialRepository.ListAsync(ct);
        return filiais.OrderByDescending(f => f.Matriz).ThenBy(f => f.Nome)
            .Select(f => new FilialDto(f.Id, f.Nome, f.Cnpj, f.Endereco, f.Cidade, f.Uf, f.Ativa, f.Matriz))
            .ToList();
    }
}
