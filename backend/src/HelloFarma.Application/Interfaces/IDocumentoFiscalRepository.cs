using HelloFarma.Domain.Entities.Fiscal;

namespace HelloFarma.Application.Interfaces;

public interface IDocumentoFiscalRepository : IRepository<DocumentoFiscal>
{
    Task<DocumentoFiscal?> ObterPorVendaAsync(Guid vendaId, CancellationToken ct = default);
}
