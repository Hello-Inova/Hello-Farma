namespace HelloFarma.Domain.Common;

/// <summary>
/// Evento de domínio base, usado para comunicação assíncrona entre módulos
/// (arquitetura event-driven, ex.: notificar Estoque quando uma Venda é concluída).
/// </summary>
public abstract record DomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
