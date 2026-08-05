using HelloFarma.Domain.Entities.Compras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.ToTable("fornecedores");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.RazaoSocial).HasMaxLength(250).IsRequired();
        builder.Property(f => f.Cnpj).HasMaxLength(20).IsRequired();
    }
}
