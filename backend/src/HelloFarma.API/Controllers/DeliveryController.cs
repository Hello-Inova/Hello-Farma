using HelloFarma.Application.UseCases.Delivery.AvancarStatusPedidoDelivery;
using HelloFarma.Application.UseCases.Delivery.CriarPedidoDelivery;
using HelloFarma.Application.UseCases.Delivery.ListarPedidosDelivery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/delivery")]
[Authorize]
public class DeliveryController(IMediator mediator) : ControllerBase
{
    /// <summary>Cria um pedido de delivery vinculado a uma venda já paga.</summary>
    [HttpPost("pedidos")]
    public async Task<IActionResult> Criar(CriarPedidoDeliveryCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Avança o status do pedido (Pendente → Separação → Expedição → Em rota → Entregue → Avaliado).</summary>
    [HttpPost("pedidos/{id:guid}/status")]
    public async Task<IActionResult> AvancarStatus(Guid id, [FromBody] AvancarStatusRequest body, CancellationToken ct)
    {
        await mediator.Send(new AvancarStatusPedidoDeliveryCommand(id, body.NovoStatus), ct);
        return NoContent();
    }

    /// <summary>Lista os pedidos de delivery em andamento — painel do entregador/expedição.</summary>
    [HttpGet("pedidos")]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var pedidos = await mediator.Send(new ListarPedidosDeliveryQuery(), ct);
        return Ok(pedidos);
    }
}

public record AvancarStatusRequest(int NovoStatus);
