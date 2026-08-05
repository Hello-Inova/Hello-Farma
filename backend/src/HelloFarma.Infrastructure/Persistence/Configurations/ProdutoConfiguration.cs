using HelloFarma.Domain.Entities.Produtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("produtos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nome).HasMaxLength(250).IsRequired();
        builder.Property(p => p.Ean).HasMaxLength(20);
        builder.Property(p => p.Pmc).HasColumnType("decimal(10,2)");
        builder.Property(p => p.Pf).HasColumnType("decimal(10,2)");
        builder.HasIndex(p => new { p.TenantId, p.Ean });
    }
}
