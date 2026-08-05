using HelloFarma.Application.UseCases.Auth.Login;
using HelloFarma.Application.UseCases.Auth.RefreshToken;
using HelloFarma.Application.UseCases.Auth.RegistrarTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>Cadastra uma nova farmácia (tenant) e seu usuário administrador inicial.</summary>
    [HttpPost("registrar-tenant")]
    public async Task<IActionResult> RegistrarTenant(RegistrarTenantCommand command, CancellationToken ct)
    {
        var tenantId = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(RegistrarTenant), new { tenantId }, new { tenantId });
    }

    /// <summary>Autentica um usuário e retorna access + refresh token.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Renova o access token a partir de um refresh token válido.</summary>
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResult>> Refresh(RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
