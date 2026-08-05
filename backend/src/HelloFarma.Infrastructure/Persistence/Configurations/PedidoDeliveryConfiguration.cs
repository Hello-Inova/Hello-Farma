using HelloFarma.Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class PedidoDeliveryConfiguration : IEntityTypeConfiguration<PedidoDelivery>
{
    public void Configure(EntityTypeBuilder<PedidoDelivery> builder)
    {
        builder.ToTable("pedidos_delivery");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.EnderecoEntrega).HasMaxLength(300).IsRequired();
        builder.HasIndex(p => new { p.TenantId, p.Status });
    }
}
