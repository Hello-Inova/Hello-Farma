using HelloFarma.Domain.Entities.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ContaFinanceiraConfiguration : IEntityTypeConfiguration<ContaFinanceira>
{
    public void Configure(EntityTypeBuilder<ContaFinanceira> builder)
    {
        builder.ToTable("contas_financeiras");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Descricao).HasMaxLength(250).IsRequired();
        builder.Property(c => c.Valor).HasColumnType("decimal(10,2)");
        builder.HasIndex(c => new { c.TenantId, c.Tipo, c.Status });
    }
}
