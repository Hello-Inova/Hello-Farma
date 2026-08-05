using MediatR;

namespace HelloFarma.Application.UseCases.Produtos.AtualizarProduto;

public record AtualizarProdutoCommand(
    Guid Id,
    string Nome,
    string? Laboratorio,
    string? PrincipioAtivo,
    string? CategoriaTerapeutica,
    string? FormaFarmaceutica,
    string? Concentracao,
    bool Controlado,
    bool ReceitaObrigatoria,
    decimal Pmc,
    decimal Pf) : IRequest<Unit>;
