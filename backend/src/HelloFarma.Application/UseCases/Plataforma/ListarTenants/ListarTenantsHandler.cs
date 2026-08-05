using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.ListarTenants;

public class ListarTenantsHandler(
    ITenantRepository tenantRepository,
    IAssinaturaRepository assinaturaRepository,
    IPlanoRepository planoRepository,
    IUsuarioRepository usuarioRepository) : IRequestHandler<ListarTenantsQuery, IReadOnlyList<TenantPlataformaDto>>
{
    public async Task<IReadOnlyList<TenantPlataformaDto>> Handle(ListarTenantsQuery request, CancellationToken ct)
    {
        var tenants = await tenantRepository.ListAsync(ct);
        var assinaturas = await assinaturaRepository.ListarMaisRecentesDeTodosOsTenantsAsync(ct);
        var planos = await planoRepository.ListAsync(ct);
        var usuariosAtivos = await usuarioRepository.ContarAtivosPorTenantAsync(ct);

        var assinaturaPorTenant = assinaturas.ToDictionary(a => a.TenantId);
        var planoPorId = planos.ToDictionary(p => p.Id);

        return tenants.Select(t =>
        {
            assinaturaPorTenant.TryGetValue(t.Id, out var assinatura);

            string? planoNome = null;
            if (assinatura is not null && planoPorId.TryGetValue(assinatura.PlanoId, out var planoDaAssinatura))
                planoNome = planoDaAssinatura.Nome;
            else if (Guid.TryParse(t.PlanoId, out var planoIdDoTenant) && planoPorId.TryGetValue(planoIdDoTenant, out var planoDoTenant))
                planoNome = planoDoTenant.Nome;

            usuariosAtivos.TryGetValue(t.Id, out var totalUsuarios);

            return new TenantPlataformaDto(
                t.Id, t.RazaoSocial, t.NomeFantasia, t.Cnpj, t.Ativo,
                planoNome, assinatura is not null ? (int)assinatura.Status : null,
                totalUsuarios, t.CreatedAtUtc);
        }).OrderByDescending(t => t.CreatedAtUtc).ToList();
    }
}
