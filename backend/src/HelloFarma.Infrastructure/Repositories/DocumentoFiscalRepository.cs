using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Fiscal;
using HelloFarma.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Repositories;

public class DocumentoFiscalRepository(HelloFarmaDbContext context) : EfRepository<DocumentoFiscal>(context), IDocumentoFiscalRepository
{
    public async Task<DocumentoFiscal?> ObterPorVendaAsync(Guid vendaId, CancellationToken ct = default) =>
        await context.DocumentosFiscais.FirstOrDefaultAsync(d => d.VendaId == vendaId && !d.IsDeleted, ct);
}
