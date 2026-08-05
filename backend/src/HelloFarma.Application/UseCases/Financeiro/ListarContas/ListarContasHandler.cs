using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.ListarContas;

public class ListarContasHandler(IContaFinanceiraRepository repository) : IRequestHandler<ListarContasQuery, IReadOnlyList<ContaFinanceiraDto>>
{
    public async Task<IReadOnlyList<ContaFinanceiraDto>> Handle(ListarContasQuery request, CancellationToken ct)
    {
        var contas = await repository.ListarAsync(
            request.Tipo.HasValue ? (TipoContaFinanceira)request.Tipo.Value : null,
            request.Status.HasValue ? (StatusContaFinanceira)request.Status.Value : null,
            ct);

        return contas.Select(c => new ContaFinanceiraDto(c.Id, (int)c.Tipo, c.Descricao, c.Valor, c.DataVencimento, c.PagaEmUtc, (int)c.Status)).ToList();
    }
}
