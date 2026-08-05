using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.IA.PreverVendas;

/// <summary>
/// Módulo Hello Farma IA — v1 heurística: projeta vendas dos próximos 7 dias com base
/// na média móvel dos últimos 30 dias. Serve de base para uma evolução futura com
/// modelos estatísticos/ML mais sofisticados, sem alterar o contrato do caso de uso.
/// </summary>
public record PreverVendasQuery : IRequest<PrevisaoVendasDto>;
