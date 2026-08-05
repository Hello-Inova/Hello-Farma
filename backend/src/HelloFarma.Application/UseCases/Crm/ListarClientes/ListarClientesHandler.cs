using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Crm.ListarClientes;

public class ListarClientesHandler(IClienteRepository repository) : IRequestHandler<ListarClientesQuery, IReadOnlyList<ClienteDto>>
{
    public async Task<IReadOnlyList<ClienteDto>> Handle(ListarClientesQuery request, CancellationToken ct)
    {
        var clientes = string.IsNullOrWhiteSpace(request.Busca)
            ? await repository.ListAsync(ct)
            : await repository.BuscarAsync(request.Busca, ct);

        return clientes.Select(c => new ClienteDto(c.Id, c.Nome, c.Cpf, c.Telefone, c.Email, c.SaldoCashback)).ToList();
    }
}
