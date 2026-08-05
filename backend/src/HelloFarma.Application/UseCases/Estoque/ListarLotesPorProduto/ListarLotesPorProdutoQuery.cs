using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.ListarLotesPorProduto;

public record ListarLotesPorProdutoQuery(Guid ProdutoId) : IRequest<IReadOnlyList<LoteDto>>;
