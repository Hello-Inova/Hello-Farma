using HelloFarma.Application.UseCases.Estoque.ListarLotesPorProduto;
using HelloFarma.Application.UseCases.Estoque.ListarLotesProximosVencimento;
using HelloFarma.Application.UseCases.Estoque.RegistrarEntrada;
using HelloFarma.Application.UseCases.Estoque.RegistrarSaida;
using HelloFarma.Application.UseCases.Estoque.TransferirEstoque;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/estoque")]
[Authorize]
public class EstoqueController(IMediator mediator) : ControllerBase
{
    /// <summary>Registra entrada de estoque (compra, ajuste positivo, devolução de cliente).</summary>
    [HttpPost("entradas")]
    public async Task<IActionResult> RegistrarEntrada(RegistrarEntradaCommand command, CancellationToken ct)
    {
        var loteId = await mediator.Send(command, ct);
        return Ok(new { loteId });
    }

    /// <summary>Registra saída de estoque (venda, ajuste negativo, perda) seguindo a regra FEFO.</summary>
    [HttpPost("saidas")]
    public async Task<IActionResult> RegistrarSaida(RegistrarSaidaCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Transfere estoque de um lote entre duas filiais.</summary>
    [HttpPost("transferencias")]
    public async Task<IActionResult> Transferir(TransferirEstoqueCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Lista os lotes de um produto, ordenados por validade (FEFO). Filtre por filial com ?filialId=.</summary>
    [HttpGet("produtos/{produtoId:guid}/lotes")]
    public async Task<IActionResult> ListarLotesPorProduto(Guid produtoId, [FromQuery] Guid? filialId, CancellationToken ct)
    {
        var lotes = await mediator.Send(new ListarLotesPorProdutoQuery(produtoId, filialId), ct);
        return Ok(lotes);
    }

    /// <summary>Lista lotes próximos do vencimento (padrão: 90 dias) — alerta de ruptura por validade.</summary>
    [HttpGet("lotes/proximos-vencimento")]
    public async Task<IActionResult> ListarProximosVencimento([FromQuery] int diasAlerta = 90, CancellationToken ct = default)
    {
        var lotes = await mediator.Send(new ListarLotesProximosVencimentoQuery(diasAlerta), ct);
        return Ok(lotes);
    }
}
