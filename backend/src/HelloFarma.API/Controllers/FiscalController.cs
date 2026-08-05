using HelloFarma.Application.UseCases.Fiscal.EmitirDocumentoFiscal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloFarma.API.Controllers;

[ApiController]
[Route("api/v1/fiscal")]
[Authorize]
public class FiscalController(IMediator mediator) : ControllerBase
{
    /// <summary>Emite o documento fiscal (NFC-e) de uma venda. Usa um emissor simulado
    /// até a integração real com SEFAZ/SAT ser conectada (Strategy Pattern plugável).</summary>
    [HttpPost("vendas/{vendaId:guid}/emitir")]
    public async Task<IActionResult> Emitir(Guid vendaId, CancellationToken ct)
    {
        await mediator.Send(new EmitirDocumentoFiscalCommand(vendaId), ct);
        return NoContent();
    }
}
