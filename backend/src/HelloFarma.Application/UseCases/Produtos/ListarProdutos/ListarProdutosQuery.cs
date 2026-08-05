using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.ListarProdutos;

public record ListarProdutosQuery(string? Busca = null) : IRequest<IReadOnlyList<ProdutoDto>>;
