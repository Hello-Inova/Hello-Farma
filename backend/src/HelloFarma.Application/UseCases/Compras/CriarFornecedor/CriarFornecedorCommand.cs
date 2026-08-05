using MediatR;

namespace HelloFarma.Application.UseCases.Compras.CriarFornecedor;

public record CriarFornecedorCommand(string RazaoSocial, string Cnpj, string? Contato, string? Telefone) : IRequest<Guid>;
