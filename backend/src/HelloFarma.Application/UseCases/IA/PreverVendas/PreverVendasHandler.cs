using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.IA.PreverVendas;

public class PreverVendasHandler(IVendaRepository vendaRepository) : IRequestHandler<PreverVendasQuery, PrevisaoVendasDto>
{
    public async Task<PrevisaoVendasDto> Handle(PreverVendasQuery request, CancellationToken ct)
    {
        var fim = DateTime.UtcNow;
        var inicio = fim.AddDays(-30);

        var vendas = await vendaRepository.ListarPorPeriodoAsync(inicio, fim, ct);
        var totalVendido = vendas.Sum(v => v.ValorTotal);
        var mediaDiaria = totalVendido / 30m;

        return new PrevisaoVendasDto(Math.Round(mediaDiaria, 2), Math.Round(mediaDiaria * 7, 2), 30);
    }
}
