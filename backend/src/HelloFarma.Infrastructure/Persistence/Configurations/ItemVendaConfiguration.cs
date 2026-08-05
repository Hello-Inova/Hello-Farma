using HelloFarma.Domain.Entities.Vendas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ItemVendaConfiguration : IEntityTypeConfiguration<ItemVenda>
{
    public void Configure(EntityTypeBuilder<ItemVenda> builder)
    {
        builder.ToTable("itens_venda");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");
        builder.Property(i => i.ProdutoNome).HasMaxLength(250);
        builder.Ignore(i => i.Subtotal);
    }
}
