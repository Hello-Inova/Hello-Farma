using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Tenants;

/// <summary>
/// Representa uma farmácia (Tenant) cadastrada na plataforma Hello Farma.
/// </summary>
public class Tenant : BaseEntity
{
    public string RazaoSocial { get; private set; } = default!;
    public string NomeFantasia { get; private set; } = default!;
    public string Cnpj { get; private set; } = default!;
    public string PlanoId { get; private set; } = default!;
    public bool Ativo { get; private set; } = true;

    protected Tenant() { }

    public Tenant(string razaoSocial, string nomeFantasia, string cnpj, string planoId)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        PlanoId = planoId;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
