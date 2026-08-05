using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.CriarProduto;

public record CriarProdutoCommand(
    string Nome,
    string Ean,
    int TipoProduto,
    decimal Pmc,
    decimal Pf,
    bool Controlado,
    bool ReceitaObrigatoria) : IRequest<Guid>;
