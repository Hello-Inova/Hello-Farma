using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.ListarVendasDoDia;

public record ListarVendasDoDiaQuery : IRequest<IReadOnlyList<VendaDto>>;
