using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Compras;

public class Fornecedor : BaseEntity
{
    public string RazaoSocial { get; private set; } = default!;
    public string Cnpj { get; private set; } = default!;
    public string? Contato { get; private set; }
    public string? Telefone { get; private set; }
    public bool Ativo { get; private set; } = true;

    protected Fornecedor() { }

    public Fornecedor(Guid tenantId, string razaoSocial, string cnpj, string? contato, string? telefone)
    {
        TenantId = tenantId;
        RazaoSocial = razaoSocial;
        Cnpj = cnpj;
        Contato = contato;
        Telefone = telefone;
    }
}
