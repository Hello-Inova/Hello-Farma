using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Estoque;

/// <summary>
/// Lote de um produto em estoque. Controle de validade e quantidade é feito
/// sempre no nível de lote, nunca apenas no produto — exigência regulatória do setor.
/// Pode estar vinculado a uma Filial específica (estoque por unidade); quando
/// FilialId é nulo, o lote é tratado como estoque único/central do tenant.
/// </summary>
public class Lote : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public Guid? FilialId { get; private set; }
    public string NumeroLote { get; private set; } = default!;
    public DateOnly Validade { get; private set; }
    public int QuantidadeAtual { get; private set; }
    public string? Localizacao { get; private set; }

    protected Lote() { }

    public Lote(Guid tenantId, Guid produtoId, string numeroLote, DateOnly validade, int quantidadeInicial, string? localizacao = null, Guid? filialId = null)
    {
        TenantId = tenantId;
        ProdutoId = produtoId;
        FilialId = filialId;
        NumeroLote = numeroLote;
        Validade = validade;
        QuantidadeAtual = quantidadeInicial;
        Localizacao = localizacao;
    }

    public bool VencidoEm(DateOnly data) => Validade < data;

    public bool ProximoDoVencimento(DateOnly data, int diasAlerta = 90) =>
        !VencidoEm(data) && Validade <= data.AddDays(diasAlerta);

    public void Adicionar(int quantidade)
    {
        if (quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser positiva.");
        QuantidadeAtual += quantidade;
        Touch();
    }

    public void Baixar(int quantidade)
    {
        if (quantidade <= 0) throw new InvalidOperationException("Quantidade deve ser positiva.");
        if (quantidade > QuantidadeAtual) throw new InvalidOperationException("Estoque insuficiente neste lote.");
        QuantidadeAtual -= quantidade;
        Touch();
    }
}
