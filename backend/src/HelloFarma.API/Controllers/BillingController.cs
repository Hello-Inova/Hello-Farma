using HelloFarma.Application.UseCases.Billing.CancelarAssinatura;
using HelloFarma.Application.UseCases.Billing.CriarAssinatura;
using HelloFarma.Application.UseCases.Billing.ListarPlanos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/billing")]
public class BillingController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista os planos comerciais disponíveis (público, usado na tela de cadastro).</summary>
    [HttpGet("planos")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarPlanos(CancellationToken ct)
    {
        var planos = await mediator.Send(new ListarPlanosQuery(), ct);
        return Ok(planos);
    }

    /// <summary>Inicia a assinatura da farmácia autenticada em um plano.</summary>
    [HttpPost("assinaturas")]
    [Authorize]
    public async Task<IActionResult> CriarAssinatura(CriarAssinaturaCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Cancela uma assinatura.</summary>
    [HttpPost("assinaturas/{id:guid}/cancelar")]
    [Authorize]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new CancelarAssinaturaCommand(id), ct);
        return NoContent();
    }
}
