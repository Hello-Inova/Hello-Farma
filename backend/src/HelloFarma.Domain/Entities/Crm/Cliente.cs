using HelloFarma.Domain.Common;

namespace HelloFarma.Domain.Entities.Crm;

/// <summary>
/// Cliente da farmácia. Base do CRM: histórico de compras, cashback e fidelização
/// são construídos a partir daqui.
/// </summary>
public class Cliente : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public string? Cpf { get; private set; }
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }
    public decimal SaldoCashback { get; private set; }

    protected Cliente() { }

    public Cliente(Guid tenantId, string nome, string? cpf, string? telefone, string? email)
    {
        TenantId = tenantId;
        Nome = nome;
        Cpf = cpf;
        Telefone = telefone;
        Email = email;
    }

    public void AcumularCashback(decimal valor)
    {
        if (valor <= 0) return;
        SaldoCashback += valor;
        Touch();
    }

    public void ResgatarCashback(decimal valor)
    {
        if (valor > SaldoCashback) throw new InvalidOperationException("Saldo de cashback insuficiente.");
        SaldoCashback -= valor;
        Touch();
    }
}
