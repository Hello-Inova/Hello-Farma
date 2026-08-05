using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.CriarProduto;

public class CriarProdutoHandler(IRepository<Produto> repository, ICurrentTenant currentTenant)
    : IRequestHandler<CriarProdutoCommand, Guid>
{
    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = new Produto(
            currentTenant.TenantId,
            request.Nome,
            request.Ean,
            (TipoProduto)request.TipoProduto,
            request.Pmc,
            request.Pf,
            request.Controlado,
            request.ReceitaObrigatoria);

        await repository.AddAsync(produto, cancellationToken);

        return produto.Id;
    }
}
