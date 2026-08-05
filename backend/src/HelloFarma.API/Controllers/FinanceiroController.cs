using HelloFarma.Application.UseCases.Financeiro.BaixarConta;
using HelloFarma.Application.UseCases.Financeiro.CriarConta;
using HelloFarma.Application.UseCases.Financeiro.ListarContas;
using HelloFarma.Application.UseCases.Financeiro.ObterFluxoCaixa;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/financeiro")]
[Authorize]
public class FinanceiroController(IMediator mediator) : ControllerBase
{
    /// <summary>Lança uma conta a pagar ou a receber manualmente.</summary>
    [HttpPost("contas")]
    public async Task<IActionResult> CriarConta(CriarContaCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Marca uma conta como paga.</summary>
    [HttpPost("contas/{id:guid}/baixar")]
    public async Task<IActionResult> Baixar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new BaixarContaCommand(id), ct);
        return NoContent();
    }

    /// <summary>Lista contas financeiras, com filtro opcional por tipo (1=Receber, 2=Pagar) e status.</summary>
    [HttpGet("contas")]
    public async Task<IActionResult> Listar([FromQuery] int? tipo, [FromQuery] int? status, CancellationToken ct)
    {
        var contas = await mediator.Send(new ListarContasQuery(tipo, status), ct);
        return Ok(contas);
    }

    /// <summary>Resumo do fluxo de caixa do mês corrente (entradas x saídas).</summary>
    [HttpGet("fluxo-caixa")]
    public async Task<IActionResult> FluxoCaixa(CancellationToken ct)
    {
        var resumo = await mediator.Send(new ObterFluxoCaixaQuery(), ct);
        return Ok(resumo);
    }
}
