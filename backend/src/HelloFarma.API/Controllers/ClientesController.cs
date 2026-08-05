using HelloFarma.Application.UseCases.Crm.CriarCliente;
using HelloFarma.Application.UseCases.Crm.ListarClientes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/clientes")]
[Authorize]
public class ClientesController(IMediator mediator) : ControllerBase
{
    /// <summary>Cadastra um cliente da farmácia (base do CRM).</summary>
    [HttpPost]
    public async Task<IActionResult> Criar(CriarClienteCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Lista clientes, com busca opcional por nome/CPF.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? busca, CancellationToken ct)
    {
        var clientes = await mediator.Send(new ListarClientesQuery(busca), ct);
        return Ok(clientes);
    }
}
