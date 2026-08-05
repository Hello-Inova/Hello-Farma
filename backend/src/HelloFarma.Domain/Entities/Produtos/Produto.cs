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
    public bool Ativo { get; private set; } = true;

    protected Produto() { }

    public Produto(
        Guid tenantId,
        string nome,
        string ean,
        TipoProduto tipoProduto,
        decimal pmc,
        decimal pf,
        bool controlado = false,
        bool receitaObrigatoria = false,
        string? registroAnvisa = null,
        string? laboratorio = null,
        string? principioAtivo = null,
        string? categoriaTerapeutica = null,
        string? formaFarmaceutica = null,
        string? concentracao = null)
    {
        TenantId = tenantId;
        Nome = nome;
        Ean = ean;
        TipoProduto = tipoProduto;
        Pmc = pmc;
        Pf = pf;
        Controlado = controlado;
        ReceitaObrigatoria = receitaObrigatoria;
        RegistroAnvisa = registroAnvisa;
        Laboratorio = laboratorio;
        PrincipioAtivo = principioAtivo;
        CategoriaTerapeutica = categoriaTerapeutica;
        FormaFarmaceutica = formaFarmaceutica;
        Concentracao = concentracao;
    }

    public void AtualizarPreco(decimal pmc, decimal pf)
    {
        Pmc = pmc;
        Pf = pf;
        Touch();
    }

    public void AtualizarDados(
        string nome,
        string? laboratorio,
        string? principioAtivo,
        string? categoriaTerapeutica,
        string? formaFarmaceutica,
        string? concentracao,
        bool controlado,
        bool receitaObrigatoria)
    {
        Nome = nome;
        Laboratorio = laboratorio;
        PrincipioAtivo = principioAtivo;
        CategoriaTerapeutica = categoriaTerapeutica;
        FormaFarmaceutica = formaFarmaceutica;
        Concentracao = concentracao;
        Controlado = controlado;
        ReceitaObrigatoria = receitaObrigatoria;
        Touch();
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
