using HelloFarma.Application.UseCases.Plataforma.AtivarTenant;
using HelloFarma.Application.UseCases.Plataforma.ListarTenants;
using HelloFarma.Application.UseCases.Plataforma.SuspenderTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

/// <summary>
/// Hello Platform — painel administrativo da Hello Inova. Acesso restrito a usuários
/// com papel SuperAdmin, que enxergam e administram todos os tenants da plataforma.
/// </summary>
[ApiController]
[Route("api/v1/plataforma")]
[Authorize(Roles = "SuperAdmin")]
public class PlataformaController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista todas as farmácias cadastradas na plataforma, com plano, status e uso.</summary>
    [HttpGet("tenants")]
    public async Task<IActionResult> ListarTenants(CancellationToken ct)
    {
        var tenants = await mediator.Send(new ListarTenantsQuery(), ct);
        return Ok(tenants);
    }

    /// <summary>Suspende o acesso de uma farmácia à plataforma (bloqueia login imediatamente).</summary>
    [HttpPost("tenants/{tenantId:guid}/suspender")]
    public async Task<IActionResult> Suspender(Guid tenantId, CancellationToken ct)
    {
        await mediator.Send(new SuspenderTenantCommand(tenantId), ct);
        return NoContent();
    }

    /// <summary>Reativa o acesso de uma farmácia previamente suspensa.</summary>
    [HttpPost("tenants/{tenantId:guid}/ativar")]
    public async Task<IActionResult> Ativar(Guid tenantId, CancellationToken ct)
    {
        await mediator.Send(new AtivarTenantCommand(tenantId), ct);
        return NoContent();
    }
}
