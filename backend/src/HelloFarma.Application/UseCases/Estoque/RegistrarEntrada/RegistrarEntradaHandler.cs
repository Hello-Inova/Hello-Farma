using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarEntrada;

public class RegistrarEntradaHandler(IEntradaEstoqueService entradaEstoqueService, IUnitOfWork unitOfWork)
    : IRequestHandler<RegistrarEntradaCommand, Guid>
{
    public async Task<Guid> Handle(RegistrarEntradaCommand request, CancellationToken ct)
    {
        var loteId = await entradaEstoqueService.EntrarAsync(
            request.ProdutoId, request.NumeroLote, request.Validade, request.Quantidade,
            request.Localizacao, request.Motivo ?? "Entrada de estoque", ct);

        await unitOfWork.SaveChangesAsync(ct);

        return loteId;
    }
}
