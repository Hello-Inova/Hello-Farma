using HelloFarma.Domain.Entities.Compras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class PedidoCompraConfiguration : IEntityTypeConfiguration<PedidoCompra>
{
    public void Configure(EntityTypeBuilder<PedidoCompra> builder)
    {
        builder.ToTable("pedidos_compra");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ValorTotal).HasColumnType("decimal(10,2)");
        builder.HasMany(p => p.Itens).WithOne().HasForeignKey(i => i.PedidoCompraId).OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(PedidoCompra.Itens))!.SetPropertyAccessMode(Microsoft.EntityFrameworkCore.PropertyAccessMode.Field);
    }
}
