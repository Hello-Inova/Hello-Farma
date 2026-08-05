using HelloFarma.Domain.Entities.Vendas;

namespace HelloFarma.Application.Interfaces;

public interface IVendaRepository : IRepository<Venda>
{
    Task<IReadOnlyList<Venda>> ListarPorPeriodoAsync(DateTime inicioUtc, DateTime fimUtc, CancellationToken ct = default);
}
