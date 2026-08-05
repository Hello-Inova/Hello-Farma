using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.ObterProduto;

public record ObterProdutoQuery(Guid Id) : IRequest<ProdutoDto?>;
