using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.CriarProduto;

public record CriarProdutoCommand(
    string Nome,
    string Ean,
    int TipoProduto,
    decimal Pmc,
    decimal Pf,
    bool Controlado,
    bool ReceitaObrigatoria,
    string? RegistroAnvisa = null,
    string? Laboratorio = null,
    string? PrincipioAtivo = null,
    string? CategoriaTerapeutica = null,
    string? FormaFarmaceutica = null,
    string? Concentracao = null) : IRequest<Guid>;
