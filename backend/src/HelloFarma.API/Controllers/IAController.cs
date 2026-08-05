using HelloFarma.Application.UseCases.IA.PreverVendas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/ia")]
[Authorize]
public class IAController(IMediator mediator) : ControllerBase
{
    /// <summary>Módulo Hello Farma IA: previsão de vendas para os próximos 7 dias.</summary>
    [HttpGet("previsao-vendas")]
    public async Task<IActionResult> PreverVendas(CancellationToken ct)
    {
        var previsao = await mediator.Send(new PreverVendasQuery(), ct);
        return Ok(previsao);
    }
}
