using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Application.Interfaces;

public interface IContaFinanceiraRepository : IRepository<ContaFinanceira>
{
    Task<IReadOnlyList<ContaFinanceira>> ListarAsync(TipoContaFinanceira? tipo, StatusContaFinanceira? status, CancellationToken ct = default);
    Task<IReadOnlyList<ContaFinanceira>> ListarPagasNoPeriodoAsync(DateTime inicioUtc, DateTime fimUtc, CancellationToken ct = default);
}
