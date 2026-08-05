namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Abstrai o SaveChanges do EF Core para a camada de aplicação (Repository + Unit of Work Pattern).
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
