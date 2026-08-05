namespace HelloFarma.Domain.Common;

/// <summary>
/// Entidade base multi-tenant. Todo agregado do Hello Farma herda desta classe
/// para garantir isolamento de dados entre farmácias (tenants).
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador do Tenant (farmácia). Obrigatório em toda entidade,
    /// conforme diretriz de isolamento multi-empresa do Hello Farma.
    /// </summary>
    public Guid TenantId { get; protected set; }

    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; protected set; }
    public bool IsDeleted { get; protected set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void Touch() => UpdatedAtUtc = DateTime.UtcNow;

    public void MarkAsDeleted() => IsDeleted = true;
}
