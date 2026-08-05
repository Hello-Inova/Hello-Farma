using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.ObterProduto;

public class ObterProdutoHandler(IProdutoRepository produtoRepository) : IRequestHandler<ObterProdutoQuery, ProdutoDto?>
{
    public async Task<ProdutoDto?> Handle(ObterProdutoQuery request, CancellationToken ct)
    {
        var p = await produtoRepository.GetByIdAsync(request.Id, ct);
        if (p is null) return null;

        return new ProdutoDto(
            p.Id, p.Nome, p.Ean, p.RegistroAnvisa, p.Laboratorio, p.PrincipioAtivo,
            p.CategoriaTerapeutica, p.FormaFarmaceutica, p.Concentracao,
            (int)p.TipoProduto, p.Controlado, p.ReceitaObrigatoria, p.Pmc, p.Pf, p.Ativo);
    }
}
