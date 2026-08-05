namespace HelloFarma.Application.DTOs;

public record ProdutoDto(
    Guid Id,
    string Nome,
    string Ean,
    string? RegistroAnvisa,
    string? Laboratorio,
    string? PrincipioAtivo,
    string? CategoriaTerapeutica,
    string? FormaFarmaceutica,
    string? Concentracao,
    int TipoProduto,
    bool Controlado,
    bool ReceitaObrigatoria,
    decimal Pmc,
    decimal Pf,
    bool Ativo);
