using HelloFarma.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.RazaoSocial).HasMaxLength(250).IsRequired();
        builder.Property(t => t.NomeFantasia).HasMaxLength(250).IsRequired();
        builder.Property(t => t.Cnpj).HasMaxLength(20).IsRequired();
        builder.HasIndex(t => t.Cnpj).IsUnique();
    }
}
