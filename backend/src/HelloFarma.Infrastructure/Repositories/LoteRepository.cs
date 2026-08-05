using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class LoteRepository(HelloFarmaDbContext context) : EfRepository<Lote>(context), ILoteRepository
{
    public async Task<Lote?> ObterPorNumeroAsync(Guid produtoId, string numeroLote, Guid? filialId, CancellationToken ct = default) =>
        await context.Lotes.FirstOrDefaultAsync(l =>
            l.ProdutoId == produtoId && l.NumeroLote == numeroLote && l.FilialId == filialId && !l.IsDeleted, ct);

    public async Task<IReadOnlyList<Lote>> ListarPorProdutoAsync(Guid produtoId, Guid? filialId, CancellationToken ct = default)
    {
        var query = context.Lotes.Where(l => l.ProdutoId == produtoId && !l.IsDeleted);
        if (filialId.HasValue)
            query = query.Where(l => l.FilialId == filialId);

        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Lote>> ListarProximosDoVencimentoAsync(int diasAlerta, CancellationToken ct = default)
    {
        var limite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(diasAlerta);
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        return await context.Lotes
            .Where(l => !l.IsDeleted && l.QuantidadeAtual > 0 && l.Validade >= hoje && l.Validade <= limite)
            .ToListAsync(ct);
    }
}
