using HelloFarma.Application.UseCases.Produtos.AtualizarProduto;
using HelloFarma.Application.UseCases.Produtos.CriarProduto;
using HelloFarma.Application.UseCases.Produtos.DesativarProduto;
using HelloFarma.Application.UseCases.Produtos.ListarProdutos;
using HelloFarma.Application.UseCases.Produtos.ObterProduto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProdutosController(IMediator mediator) : ControllerBase
{
    /// <summary>Lista os produtos do tenant autenticado, com busca opcional por nome/EAN.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? busca, CancellationToken ct)
    {
        var produtos = await mediator.Send(new ListarProdutosQuery(busca), ct);
        return Ok(produtos);
    }

    /// <summary>Obtém um produto pelo Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
    {
        var produto = await mediator.Send(new ObterProdutoQuery(id), ct);
        return produto is null ? NotFound() : Ok(produto);
    }

    /// <summary>Cadastra um novo produto farmacêutico para o tenant autenticado.</summary>
    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    /// <summary>Atualiza os dados de um produto existente.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("Id da rota difere do corpo da requisição.");
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Desativa (soft) um produto — não exclui fisicamente por questões de auditoria.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DesativarProdutoCommand(id), ct);
        return NoContent();
    }
}
