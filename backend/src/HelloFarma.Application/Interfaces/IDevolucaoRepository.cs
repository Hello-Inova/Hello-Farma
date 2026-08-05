using HelloFarma.Domain.Entities.Vendas;

namespace HelloFarma.Application.Interfaces;

public interface IDevolucaoRepository : IRepository<Devolucao>
{
    Task<IReadOnlyList<Devolucao>> ListarPorVendaAsync(Guid vendaId, CancellationToken ct = default);
}
