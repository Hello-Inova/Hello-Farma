using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Billing;
using HelloFarma.Domain.Entities.Tenants;
using HelloFarma.Domain.Entities.Usuarios;
using HelloFarma.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Persistence.Seed;

/// <summary>Popula os planos comerciais padrão e o acesso inicial à Hello Platform na primeira execução.</summary>
public static class DbSeeder
{
    private const string CnpjHelloInova = "67883808000135";
    private const string EmailSuperAdmin = "admin@helloinova.com.br";

    /// <summary>Senha inicial do super-admin da Hello Platform — deve ser trocada após o primeiro acesso.</summary>
    public const string SenhaInicialSuperAdmin = "HelloInova@2026";

    public static async Task SeedAsync(HelloFarmaDbContext context, IPasswordHasher passwordHasher)
    {
        if (!await context.Planos.IgnoreQueryFilters().AnyAsync())
        {
            context.Planos.AddRange(
                new Plano(Guid.Empty, "Mensal", 199.90m, 5, 1, 500, permiteDelivery: true, permiteIa: false),
                new Plano(Guid.Empty, "Semestral", 179.90m, 10, 3, 2000, permiteDelivery: true, permiteIa: true),
                new Plano(Guid.Empty, "Anual", 149.90m, 999, 999, 999999, permiteDelivery: true, permiteIa: true));

            await context.SaveChangesAsync();
        }

        var tenantHelloInova = await context.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Cnpj == CnpjHelloInova);
        if (tenantHelloInova is null)
        {
            var planoInterno = await context.Planos.IgnoreQueryFilters().OrderByDescending(p => p.LimiteUsuarios).FirstAsync();
            tenantHelloInova = new Tenant("Hello Inova Tecnologia LTDA", "Hello Inova", CnpjHelloInova, planoInterno.Id.ToString());
            context.Tenants.Add(tenantHelloInova);
            await context.SaveChangesAsync();
        }

        var superAdminExiste = await context.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.Papel == PapelUsuario.SuperAdmin);
        if (!superAdminExiste)
        {
            var senhaHash = passwordHasher.Hash(SenhaInicialSuperAdmin);
            var superAdmin = new Usuario(tenantHelloInova.Id, "Super Admin Hello Inova", EmailSuperAdmin, senhaHash, PapelUsuario.SuperAdmin);
            context.Usuarios.Add(superAdmin);
            await context.SaveChangesAsync();
        }
    }
}
