using HelloFarma.Domain.Entities.Compras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ItemPedidoCompraConfiguration : IEntityTypeConfiguration<ItemPedidoCompra>
{
    public void Configure(EntityTypeBuilder<ItemPedidoCompra> builder)
    {
        builder.ToTable("itens_pedido_compra");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");
        builder.Property(i => i.NumeroLote).HasMaxLength(50);
        builder.Ignore(i => i.Subtotal);
    }
}
