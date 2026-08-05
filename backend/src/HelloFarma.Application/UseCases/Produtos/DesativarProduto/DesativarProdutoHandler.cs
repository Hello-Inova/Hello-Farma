using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.DesativarProduto;

public class DesativarProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DesativarProdutoCommand, Unit>
{
    public async Task<Unit> Handle(DesativarProdutoCommand request, CancellationToken ct)
    {
        var produto = await produtoRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Produto não encontrado.");

        produto.Desativar();
        produtoRepository.Update(produto);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
