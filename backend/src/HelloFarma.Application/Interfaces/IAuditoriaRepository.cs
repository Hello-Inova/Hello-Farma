using HelloFarma.Domain.Entities.Auditoria;

namespace HelloFarma.Application.Interfaces;

public interface IAuditoriaRepository : IRepository<LogAuditoria>
{
    Task<IReadOnlyList<LogAuditoria>> ListarRecentesAsync(int quantidade, CancellationToken ct = default);
}
