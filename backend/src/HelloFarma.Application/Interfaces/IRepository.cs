using HelloFarma.Domain.Common;

namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Contrato genérico de repositório (Repository Pattern). Toda implementação
/// deve respeitar o isolamento por TenantId.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
}
