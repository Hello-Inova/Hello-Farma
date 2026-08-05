using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.ReceberPedidoCompra;

public class ReceberPedidoCompraHandler(
    IPedidoCompraRepository pedidoRepository,
    IEntradaEstoqueService entradaEstoqueService,
    IContaFinanceiraRepository contaFinanceiraRepository,
    ICurrentTenant currentTenant,
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
                null, $"Recebimento do pedido de compra {pedido.Id}", filialId: null, ct: ct);
        }

        pedidoRepository.Update(pedido);

        var conta = new ContaFinanceira(
            currentTenant.TenantId, TipoContaFinanceira.Pagar, $"Compra - pedido {pedido.Id}", pedido.ValorTotal,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)), "PedidoCompra", pedido.Id);
        await contaFinanceiraRepository.AddAsync(conta, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
