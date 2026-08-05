using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Common;
using HelloFarma.Domain.Entities.Auth;
using HelloFarma.Domain.Entities.Billing;
using HelloFarma.Domain.Entities.Fiscal;
using HelloFarma.Domain.Entities.Compras;
using HelloFarma.Domain.Entities.Crm;
using HelloFarma.Domain.Entities.Delivery;
using HelloFarma.Domain.Entities.Empresa;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Entities.Tenants;
using HelloFarma.Domain.Entities.Usuarios;
using HelloFarma.Domain.Entities.Vendas;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Persistence;

/// <summary>
/// DbContext principal. Aplica automaticamente o filtro global por TenantId em todas as
/// entidades multi-tenant (exceto Tenant, que É o tenant), garantindo isolamento de dados
/// entre farmácias — nunca compartilhar dados entre empresas.
/// </summary>
public class HelloFarmaDbContext(DbContextOptions<HelloFarmaDbContext> options, ICurrentTenant currentTenant)
    : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Lote> Lotes => Set<Lote>();
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<PedidoCompra> PedidosCompra => Set<PedidoCompra>();
    public DbSet<ItemPedidoCompra> ItensPedidoCompra => Set<ItemPedidoCompra>();
    public DbSet<ContaFinanceira> ContasFinanceiras => Set<ContaFinanceira>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PedidoDelivery> PedidosDelivery => Set<PedidoDelivery>();
    public DbSet<DocumentoFiscal> DocumentosFiscais => Set<DocumentoFiscal>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<Assinatura> Assinaturas => Set<Assinatura>();
    public DbSet<Filial> Filiais => Set<Filial>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>(); // usado só em memória para eventos de domínio; nunca deve ser persistido
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HelloFarmaDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType == typeof(Tenant)) continue;
            if (entityType.ClrType == typeof(Plano)) continue; // catálogo global da Hello Platform, não pertence a um tenant específico
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)) continue;

            var method = typeof(HelloFarmaDbContext)
                .GetMethod(nameof(BuildTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(this, new object[] { modelBuilder });
        }

        base.OnModelCreating(modelBuilder);
    }

    private void BuildTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == currentTenant.TenantId && !e.IsDeleted);
    }
}
