using HelloFarma.Domain.Entities.Fiscal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class DocumentoFiscalConfiguration : IEntityTypeConfiguration<DocumentoFiscal>
{
    public void Configure(EntityTypeBuilder<DocumentoFiscal> builder)
    {
        builder.ToTable("documentos_fiscais");
        builder.HasKey(d => d.Id);
        builder.HasIndex(d => d.VendaId).IsUnique();
    }
}
