using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Crm.ListarClientes;

public record ListarClientesQuery(string? Busca = null) : IRequest<IReadOnlyList<ClienteDto>>;
