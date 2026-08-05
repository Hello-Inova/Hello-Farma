using HelloFarma.Domain.Entities.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class AssinaturaConfiguration : IEntityTypeConfiguration<Assinatura>
{
    public void Configure(EntityTypeBuilder<Assinatura> builder)
    {
        builder.ToTable("assinaturas");
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => new { a.TenantId, a.Status });
    }
}
