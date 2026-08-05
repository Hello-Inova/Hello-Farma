using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Fiscal.EmitirDocumentoFiscal;

public class EmitirDocumentoFiscalHandler(
    IDocumentoFiscalRepository documentoRepository,
    IVendaRepository vendaRepository,
    IEmissorFiscal emissorFiscal,
    IUnitOfWork unitOfWork) : IRequestHandler<EmitirDocumentoFiscalCommand, Unit>
{
    public async Task<Unit> Handle(EmitirDocumentoFiscalCommand request, CancellationToken ct)
    {
        var documento = await documentoRepository.ObterPorVendaAsync(request.VendaId, ct)
            ?? throw new KeyNotFoundException("Documento fiscal não encontrado para esta venda.");

        var venda = await vendaRepository.GetByIdAsync(request.VendaId, ct)
            ?? throw new KeyNotFoundException("Venda não encontrada.");

        var resultado = await emissorFiscal.EmitirAsync(venda.Id, venda.ValorTotal, ct);

        if (resultado.Sucesso)
            documento.MarcarEmitido(resultado.ChaveAcesso!);
        else
            documento.MarcarRejeitado(resultado.MotivoRejeicao ?? "Rejeitado pelo emissor fiscal.");

        documentoRepository.Update(documento);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
