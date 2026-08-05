using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.TransferirEstoque;

public class TransferirEstoqueHandler(
    IBaixaEstoqueService baixaEstoqueService,
    IEntradaEstoqueService entradaEstoqueService,
    IFilialRepository filialRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<TransferirEstoqueCommand, Unit>
{
    public async Task<Unit> Handle(TransferirEstoqueCommand request, CancellationToken ct)
    {
        if (request.FilialOrigemId == request.FilialDestinoId)
            throw new InvalidOperationException("Filial de origem e destino não podem ser a mesma.");

        if (request.Quantidade <= 0)
            throw new InvalidOperationException("Quantidade deve ser positiva.");

        _ = await filialRepository.GetByIdAsync(request.FilialOrigemId, ct)
            ?? throw new KeyNotFoundException("Filial de origem não encontrada.");
        _ = await filialRepository.GetByIdAsync(request.FilialDestinoId, ct)
            ?? throw new KeyNotFoundException("Filial de destino não encontrada.");

        await baixaEstoqueService.BaixarAsync(
            request.ProdutoId, request.Quantidade, "Transferência entre filiais (saída)", request.FilialOrigemId, ct);

        await entradaEstoqueService.EntrarAsync(
            request.ProdutoId, request.NumeroLote, request.Validade, request.Quantidade,
            null, "Transferência entre filiais (entrada)", request.FilialDestinoId, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
