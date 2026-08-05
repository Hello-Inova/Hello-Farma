using HelloFarma.Domain.Entities.Vendas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ItemDevolucaoConfiguration : IEntityTypeConfiguration<ItemDevolucao>
{
    public void Configure(EntityTypeBuilder<ItemDevolucao> builder)
    {
        builder.ToTable("itens_devolucao");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");
    }
}
