using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Compras;
using MediatR;

namespace HelloFarma.Application.UseCases.Compras.CriarPedidoCompra;

public class CriarPedidoCompraHandler(
    IPedidoCompraRepository pedidoRepository,
    IProdutoRepository produtoRepository,
    ICurrentTenant currentTenant,
    IUnitOfWork unitOfWork) : IRequestHandler<CriarPedidoCompraCommand, Guid>
{
    public async Task<Guid> Handle(CriarPedidoCompraCommand request, CancellationToken ct)
    {
        var pedido = new PedidoCompra(currentTenant.TenantId, request.FornecedorId);

        foreach (var item in request.Itens)
        {
            var produto = await produtoRepository.GetByIdAsync(item.ProdutoId, ct)
                ?? throw new KeyNotFoundException($"Produto {item.ProdutoId} não encontrado.");

            pedido.AdicionarItem(produto.Id, produto.Nome, item.Quantidade, item.PrecoUnitario, item.NumeroLote, item.Validade);
        }

        pedido.ConfirmarPedido();

        await pedidoRepository.AddAsync(pedido, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return pedido.Id;
    }
}
