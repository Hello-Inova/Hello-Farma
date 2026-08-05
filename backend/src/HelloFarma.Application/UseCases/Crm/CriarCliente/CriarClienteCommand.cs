using MediatR;

namespace HelloFarma.Application.UseCases.Crm.CriarCliente;

public record CriarClienteCommand(string Nome, string? Cpf, string? Telefone, string? Email) : IRequest<Guid>;
