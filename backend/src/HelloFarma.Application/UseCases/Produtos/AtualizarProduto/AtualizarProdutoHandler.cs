using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.AtualizarProduto;

public class AtualizarProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AtualizarProdutoCommand, Unit>
{
    public async Task<Unit> Handle(AtualizarProdutoCommand request, CancellationToken ct)
    {
        var produto = await produtoRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Produto não encontrado.");

        produto.AtualizarDados(
            request.Nome, request.Laboratorio, request.PrincipioAtivo, request.CategoriaTerapeutica,
            request.FormaFarmaceutica, request.Concentracao, request.Controlado, request.ReceitaObrigatoria);
        produto.AtualizarPreco(request.Pmc, request.Pf);

        produtoRepository.Update(produto);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
