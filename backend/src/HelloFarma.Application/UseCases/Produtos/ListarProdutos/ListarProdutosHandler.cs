using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.ListarProdutos;

public class ListarProdutosHandler(IProdutoRepository produtoRepository)
    : IRequestHandler<ListarProdutosQuery, IReadOnlyList<ProdutoDto>>
{
    public async Task<IReadOnlyList<ProdutoDto>> Handle(ListarProdutosQuery request, CancellationToken ct)
    {
        var produtos = string.IsNullOrWhiteSpace(request.Busca)
            ? await produtoRepository.ListAsync(ct)
            : await produtoRepository.BuscarAsync(request.Busca, ct);

        return produtos.Select(p => new ProdutoDto(
            p.Id, p.Nome, p.Ean, p.RegistroAnvisa, p.Laboratorio, p.PrincipioAtivo,
            p.CategoriaTerapeutica, p.FormaFarmaceutica, p.Concentracao,
            (int)p.TipoProduto, p.Controlado, p.ReceitaObrigatoria, p.Pmc, p.Pf, p.Ativo)).ToList();
    }
}
