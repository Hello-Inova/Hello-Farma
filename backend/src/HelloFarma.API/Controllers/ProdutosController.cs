using HelloFarma.Application.UseCases.Produtos.CriarProduto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProdutosController(IMediator mediator) : ControllerBase
{
    /// <summary>Cadastra um novo produto farmacêutico para o tenant autenticado.</summary>
    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(Criar), new { id }, new { id });
    }
}
