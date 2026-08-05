using HelloFarma.Application.UseCases.Vendas.CriarVenda;
using HelloFarma.Application.UseCases.Vendas.ListarVendasDoDia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/vendas")]
[Authorize]
public class VendasController(IMediator mediator) : ControllerBase
{
    /// <summary>Fecha uma venda no PDV: valida itens, calcula total, baixa estoque (FEFO) e registra o pagamento.</summary>
    [HttpPost]
    public async Task<IActionResult> Criar(CriarVendaCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Lista as vendas realizadas hoje — usado no dashboard e fechamento de caixa.</summary>
    [HttpGet("hoje")]
    public async Task<IActionResult> ListarDoDia(CancellationToken ct)
    {
        var vendas = await mediator.Send(new ListarVendasDoDiaQuery(), ct);
        return Ok(vendas);
    }
}
