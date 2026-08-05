using HelloFarma.Domain.Entities.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloFarma.Infrastructure.Persistence.Configurations;

public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
{
    public void Configure(EntityTypeBuilder<LogAuditoria> builder)
    {
        builder.ToTable("logs_auditoria");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Acao).HasMaxLength(150).IsRequired();
        builder.Property(l => l.UsuarioNome).HasMaxLength(200);
        builder.Property(l => l.IpAddress).HasMaxLength(50);
        builder.HasIndex(l => new { l.TenantId, l.CreatedAtUtc });
    }
}
