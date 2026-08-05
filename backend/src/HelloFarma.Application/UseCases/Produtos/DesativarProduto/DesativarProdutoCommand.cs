using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.DesativarProduto;

public record DesativarProdutoCommand(Guid Id) : IRequest<Unit>;
