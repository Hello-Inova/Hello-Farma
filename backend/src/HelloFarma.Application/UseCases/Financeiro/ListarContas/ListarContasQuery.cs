using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.ListarContas;

public record ListarContasQuery(int? Tipo, int? Status) : IRequest<IReadOnlyList<ContaFinanceiraDto>>;
