using HelloFarma.Application.UseCases.Filiais.AtivarFilial;
using HelloFarma.Application.UseCases.Filiais.CriarFilial;
using HelloFarma.Application.UseCases.Filiais.DesativarFilial;
using HelloFarma.Application.UseCases.Filiais.ListarFiliais;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class FiliaisController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista as filiais do tenant autenticado.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var filiais = await mediator.Send(new ListarFiliaisQuery(), ct);
        return Ok(filiais);
    }

    /// <summary>Cadastra uma nova filial, respeitando o limite de filiais do plano contratado.</summary>
    [HttpPost]
    public async Task<IActionResult> Criar(CriarFilialCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Desativa uma filial (não é possível desativar a matriz).</summary>
    [HttpPost("{id:guid}/desativar")]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DesativarFilialCommand(id), ct);
        return NoContent();
    }

    /// <summary>Reativa uma filial previamente desativada.</summary>
    [HttpPost("{id:guid}/ativar")]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new AtivarFilialCommand(id), ct);
        return NoContent();
    }
}
