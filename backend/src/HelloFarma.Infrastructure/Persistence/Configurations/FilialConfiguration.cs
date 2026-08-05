using HelloFarma.Domain.Entities.Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class FilialConfiguration : IEntityTypeConfiguration<Filial>
{
    public void Configure(EntityTypeBuilder<Filial> builder)
    {
        builder.ToTable("filiais");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Nome).HasMaxLength(200).IsRequired();
        builder.Property(f => f.Cnpj).HasMaxLength(20);
        builder.Property(f => f.Uf).HasMaxLength(2);
        builder.HasIndex(f => f.TenantId);
    }
}
