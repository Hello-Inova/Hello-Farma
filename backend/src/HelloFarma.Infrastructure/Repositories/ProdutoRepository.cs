using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class ProdutoRepository(HelloFarmaDbContext context) : EfRepository<Produto>(context), IProdutoRepository
{
    public async Task<IReadOnlyList<Produto>> BuscarAsync(string termo, CancellationToken ct = default) =>
        await context.Produtos
            .Where(p => !p.IsDeleted && (p.Nome.Contains(termo) || p.Ean.Contains(termo)))
            .ToListAsync(ct);
}
