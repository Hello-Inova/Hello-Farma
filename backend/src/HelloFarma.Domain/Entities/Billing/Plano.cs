using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Billing;

/// <summary>
/// Plano comercial da plataforma Hello Farma (Hello Platform). Todo limite é
/// configurável sem alteração de código, conforme diretriz do master prompt.
/// </summary>
public class Plano : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public decimal PrecoMensal { get; private set; }
    public int LimiteUsuarios { get; private set; }
    public int LimiteFiliais { get; private set; }
    public int LimiteProdutos { get; private set; }
    public bool PermiteDelivery { get; private set; }
    public bool PermiteIA { get; private set; }

    protected Plano() { }

    public Plano(Guid tenantId, string nome, decimal precoMensal, int limiteUsuarios, int limiteFiliais, int limiteProdutos, bool permiteDelivery, bool permiteIa)
    {
        TenantId = tenantId;
        Nome = nome;
        PrecoMensal = precoMensal;
        LimiteUsuarios = limiteUsuarios;
        LimiteFiliais = limiteFiliais;
        LimiteProdutos = limiteProdutos;
        PermiteDelivery = permiteDelivery;
        PermiteIA = permiteIa;
    }
}
