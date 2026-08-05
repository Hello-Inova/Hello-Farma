using HelloFarma.Domain.Entities.Vendas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class DevolucaoConfiguration : IEntityTypeConfiguration<Devolucao>
{
    public void Configure(EntityTypeBuilder<Devolucao> builder)
    {
        builder.ToTable("devolucoes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.ValorTotal).HasColumnType("decimal(10,2)");
        builder.Property(d => d.CashbackEstornado).HasColumnType("decimal(10,2)");
        builder.HasMany(d => d.Itens).WithOne().HasForeignKey(i => i.DevolucaoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(d => new { d.TenantId, d.VendaId });
        builder.Metadata.FindNavigation(nameof(Devolucao.Itens))!.SetPropertyAccessMode(Microsoft.EntityFrameworkCore.PropertyAccessMode.Field);
    }
}
