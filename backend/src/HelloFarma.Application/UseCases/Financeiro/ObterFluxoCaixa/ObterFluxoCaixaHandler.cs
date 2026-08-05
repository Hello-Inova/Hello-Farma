using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.ObterFluxoCaixa;

public class ObterFluxoCaixaHandler(IContaFinanceiraRepository repository) : IRequestHandler<ObterFluxoCaixaQuery, FluxoCaixaDto>
{
    public async Task<FluxoCaixaDto> Handle(ObterFluxoCaixaQuery request, CancellationToken ct)
    {
        var inicio = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var fim = inicio.AddMonths(1);

        var contasPagas = await repository.ListarPagasNoPeriodoAsync(inicio, fim, ct);

        var entradas = contasPagas.Where(c => c.Tipo == TipoContaFinanceira.Receber).Sum(c => c.Valor);
        var saidas = contasPagas.Where(c => c.Tipo == TipoContaFinanceira.Pagar).Sum(c => c.Valor);

        return new FluxoCaixaDto(entradas, saidas, entradas - saidas, inicio, fim);
    }
}
