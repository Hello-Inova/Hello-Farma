using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Empresa;

/// <summary>
/// Filial (unidade física) de uma farmácia (Tenant). Uma rede pode ter várias filiais,
/// cada uma com seu próprio estoque e vendas, respeitando o limite do plano contratado.
/// </summary>
public class Filial : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public string? Cnpj { get; private set; }
    public string? Endereco { get; private set; }
    public string? Cidade { get; private set; }
    public string? Uf { get; private set; }
    public bool Ativa { get; private set; } = true;
    public bool Matriz { get; private set; }

    protected Filial() { }

    public Filial(Guid tenantId, string nome, string? cnpj, string? endereco, string? cidade, string? uf, bool matriz = false)
    {
        TenantId = tenantId;
        Nome = nome;
        Cnpj = cnpj;
        Endereco = endereco;
        Cidade = cidade;
        Uf = uf;
        Matriz = matriz;
    }

    public void Atualizar(string nome, string? cnpj, string? endereco, string? cidade, string? uf)
    {
        Nome = nome;
        Cnpj = cnpj;
        Endereco = endereco;
        Cidade = cidade;
        Uf = uf;
        Touch();
    }

    public void Desativar() => Ativa = false;
    public void Ativar() => Ativa = true;
}
