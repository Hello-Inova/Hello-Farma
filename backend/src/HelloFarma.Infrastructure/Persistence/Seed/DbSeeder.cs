using HelloFarma.Domain.Entities.Billing;
using Microsoft.EntityFrameworkCore;

namespace HelloFarma.Infrastructure.Persistence.Seed;

/// <summary>Popula os planos comerciais padrão da Hello Platform na primeira execução.</summary>
public static class DbSeeder
{
    public static async Task SeedAsync(HelloFarmaDbContext context)
    {
        if (await context.Planos.IgnoreQueryFilters().AnyAsync()) return;

        context.Planos.AddRange(
            new Plano(Guid.Empty, "Mensal", 199.90m, 5, 1, 500, permiteDelivery: true, permiteIa: false),
            new Plano(Guid.Empty, "Semestral", 179.90m, 10, 3, 2000, permiteDelivery: true, permiteIa: true),
            new Plano(Guid.Empty, "Anual", 149.90m, 999, 999, 999999, permiteDelivery: true, permiteIa: true));

        await context.SaveChangesAsync();
    }
}
