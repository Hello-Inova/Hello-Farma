using HelloFarma.Domain.Entities.Estoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class LoteConfiguration : IEntityTypeConfiguration<Lote>
{
    public void Configure(EntityTypeBuilder<Lote> builder)
    {
        builder.ToTable("lotes");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.NumeroLote).HasMaxLength(50).IsRequired();
        builder.HasIndex(l => new { l.TenantId, l.ProdutoId, l.NumeroLote });
        builder.HasIndex(l => l.FilialId);
    }
}
