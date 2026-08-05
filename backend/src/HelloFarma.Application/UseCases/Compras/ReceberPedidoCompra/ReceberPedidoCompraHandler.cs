using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.ReceberPedidoCompra;

public class ReceberPedidoCompraHandler(
    IPedidoCompraRepository pedidoRepository,
    IEntradaEstoqueService entradaEstoqueService,
    IUnitOfWork unitOfWork) : IRequestHandler<ReceberPedidoCompraCommand, Unit>
{
    public async Task<Unit> Handle(ReceberPedidoCompraCommand request, CancellationToken ct)
    {
        var pedido = await pedidoRepository.GetByIdAsync(request.PedidoCompraId, ct)
            ?? throw new KeyNotFoundException("Pedido de compra não encontrado.");

        pedido.Receber();

        foreach (var item in pedido.Itens)
        {
            await entradaEstoqueService.EntrarAsync(
                item.ProdutoId, item.NumeroLote, item.Validade, item.Quantidade,
                null, $"Recebimento do pedido de compra {pedido.Id}", ct);
        }

        pedidoRepository.Update(pedido);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
