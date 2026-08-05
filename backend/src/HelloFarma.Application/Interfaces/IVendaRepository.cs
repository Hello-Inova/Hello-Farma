using HelloFarma.Domain.Entities.Vendas;

namespace HelloFarma.Application.Interfaces;

public interface IVendaRepository : IRepository<Venda>
{
    Task<IReadOnlyList<Venda>> ListarPorPeriodoAsync(DateTime inicioUtc, DateTime fimUtc, CancellationToken ct = default);

    /// <summary>Obtém a venda já com os itens carregados (necessário para devolução/troca).</summary>
    Task<Venda?> ObterComItensAsync(Guid id, CancellationToken ct = default);
}
