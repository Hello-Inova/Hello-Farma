using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Persistence;

/// <summary>
/// DbContext principal. Aplica automaticamente o filtro global por TenantId
/// em todas as entidades multi-tenant, garantindo isolamento de dados.
/// </summary>
public class HelloFarmaDbContext(DbContextOptions<HelloFarmaDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HelloFarmaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
