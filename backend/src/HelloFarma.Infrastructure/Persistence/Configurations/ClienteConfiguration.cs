using HelloFarma.Domain.Entities.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();
        builder.Property(c => c.SaldoCashback).HasColumnType("decimal(10,2)");
        builder.HasIndex(c => new { c.TenantId, c.Cpf });
    }
}
