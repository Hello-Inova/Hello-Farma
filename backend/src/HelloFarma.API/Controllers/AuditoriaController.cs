using HelloFarma.Application.UseCases.Auditoria.ListarAuditoria;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/auditoria")]
[Authorize(Roles = "Administrador")]
public class AuditoriaController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista os registros de auditoria mais recentes do tenant (quem fez o quê e quando).</summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int quantidade = 200, CancellationToken ct = default)
    {
        var logs = await mediator.Send(new ListarAuditoriaQuery(quantidade), ct);
        return Ok(logs);
    }
}
