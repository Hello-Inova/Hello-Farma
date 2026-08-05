using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.CriarProduto;

public class CriarProdutoHandler(IProdutoRepository repository, ICurrentTenant currentTenant, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarProdutoCommand, Guid>
{
    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken ct)
    {
        var produto = new Produto(
            currentTenant.TenantId,
            request.Nome,
            request.Ean,
            (TipoProduto)request.TipoProduto,
            request.Pmc,
            request.Pf,
            request.Controlado,
            request.ReceitaObrigatoria,
            request.RegistroAnvisa,
            request.Laboratorio,
            request.PrincipioAtivo,
            request.CategoriaTerapeutica,
            request.FormaFarmaceutica,
            request.Concentracao);

        await repository.AddAsync(produto, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return produto.Id;
    }
}
