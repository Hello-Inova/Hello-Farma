using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.ObterFluxoCaixa;

/// <summary>Resumo do fluxo de caixa (entradas x saídas) do mês corrente.</summary>
public record ObterFluxoCaixaQuery : IRequest<FluxoCaixaDto>;
