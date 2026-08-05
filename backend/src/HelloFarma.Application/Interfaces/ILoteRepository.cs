using HelloFarma.Domain.Entities.Estoque;

namespace HelloFarma.Application.Interfaces;

public interface ILoteRepository : IRepository<Lote>
{
    Task<Lote?> ObterPorNumeroAsync(Guid produtoId, string numeroLote, Guid? filialId, CancellationToken ct = default);
    Task<IReadOnlyList<Lote>> ListarPorProdutoAsync(Guid produtoId, Guid? filialId, CancellationToken ct = default);
    Task<IReadOnlyList<Lote>> ListarProximosDoVencimentoAsync(int diasAlerta, CancellationToken ct = default);
}
