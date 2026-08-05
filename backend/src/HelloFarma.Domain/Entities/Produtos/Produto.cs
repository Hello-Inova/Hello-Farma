using HelloFarma.Domain.Common;
using HelloFarma.Domain.Enums;

namespace HelloFarma.Domain.Entities.Produtos;

/// <summary>
/// Produto farmacêutico. Nunca deve ser tratado como um produto genérico de varejo:
/// carrega os atributos regulatórios e operacionais exigidos pelo setor.
/// </summary>
public class Produto : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public string Ean { get; private set; } = default!;
    public string? RegistroAnvisa { get; private set; }
    public string? Laboratorio { get; private set; }
    public string? PrincipioAtivo { get; private set; }
    public string? CategoriaTerapeutica { get; private set; }
    public string? FormaFarmaceutica { get; private set; }
    public string? Concentracao { get; private set; }
    public TipoProduto TipoProduto { get; private set; }
    public bool Controlado { get; private set; }
    public bool ReceitaObrigatoria { get; private set; }
    public decimal Pmc { get; private set; }
    public decimal Pf { get; private set; }

    protected Produto() { }

    public Produto(
        Guid tenantId,
        string nome,
        string ean,
        TipoProduto tipoProduto,
        decimal pmc,
        decimal pf,
        bool controlado = false,
        bool receitaObrigatoria = false)
    {
        TenantId = tenantId;
        Nome = nome;
        Ean = ean;
        TipoProduto = tipoProduto;
        Pmc = pmc;
        Pf = pf;
        Controlado = controlado;
        ReceitaObrigatoria = receitaObrigatoria;
    }

    public void AtualizarPreco(decimal pmc, decimal pf)
    {
        Pmc = pmc;
        Pf = pf;
        Touch();
    }
}
