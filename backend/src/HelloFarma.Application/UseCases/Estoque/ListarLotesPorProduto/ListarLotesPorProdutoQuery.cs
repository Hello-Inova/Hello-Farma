using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.ListarLotesPorProduto;

public record ListarLotesPorProdutoQuery(Guid ProdutoId, Guid? FilialId = null) : IRequest<IReadOnlyList<LoteDto>>;
