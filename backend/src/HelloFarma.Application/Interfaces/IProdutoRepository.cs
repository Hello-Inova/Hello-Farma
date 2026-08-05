using HelloFarma.Domain.Entities.Produtos;

namespace HelloFarma.Application.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IReadOnlyList<Produto>> BuscarAsync(string termo, CancellationToken ct = default);
}
