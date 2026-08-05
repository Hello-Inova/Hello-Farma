using MediatR;

namespace HelloFarma.Application.UseCases.Fiscal.EmitirDocumentoFiscal;

public record EmitirDocumentoFiscalCommand(Guid VendaId) : IRequest<Unit>;
