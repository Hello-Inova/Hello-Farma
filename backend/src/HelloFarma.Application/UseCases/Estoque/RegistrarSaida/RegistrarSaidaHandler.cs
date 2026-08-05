using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.RegistrarSaida;

public class RegistrarSaidaHandler(IBaixaEstoqueService baixaEstoqueService, IUnitOfWork unitOfWork)
    : IRequestHandler<RegistrarSaidaCommand, Unit>
{
    public async Task<Unit> Handle(RegistrarSaidaCommand request, CancellationToken ct)
    {
        await baixaEstoqueService.BaixarAsync(request.ProdutoId, request.Quantidade, request.Motivo ?? "Saída de estoque", ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
