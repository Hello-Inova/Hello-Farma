using HelloFarma.Application.UseCases.Compras.CriarFornecedor;
using HelloFarma.Application.UseCases.Compras.CriarPedidoCompra;
using HelloFarma.Application.UseCases.Compras.ListarPedidosCompra;
using HelloFarma.Application.UseCases.Compras.ReceberPedidoCompra;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/compras")]
[Authorize]
public class ComprasController(IMediator mediator) : ControllerBase
{
    /// <summary>Cadastra um fornecedor.</summary>
    [HttpPost("fornecedores")]
    public async Task<IActionResult> CriarFornecedor(CriarFornecedorCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Cria e confirma um pedido de compra junto a um fornecedor (fluxo Cotação → Pedido).</summary>
    [HttpPost("pedidos")]
    public async Task<IActionResult> CriarPedido(CriarPedidoCompraCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(new { id });
    }

    /// <summary>Confirma o recebimento físico do pedido e dá entrada em estoque (fluxo Recebimento → Entrada).</summary>
    [HttpPost("pedidos/{id:guid}/receber")]
    public async Task<IActionResult> Receber(Guid id, CancellationToken ct)
    {
        await mediator.Send(new ReceberPedidoCompraCommand(id), ct);
        return NoContent();
    }

    /// <summary>Lista os pedidos de compra.</summary>
    [HttpGet("pedidos")]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var pedidos = await mediator.Send(new ListarPedidosCompraQuery(), ct);
        return Ok(pedidos);
    }
}
