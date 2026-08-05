namespace HelloFarma.Domain.Enums;

/// <summary>
/// Papéis de usuário dentro de um Tenant (farmácia). Controla permissões de acesso
/// aos módulos do ERP.
/// </summary>
public enum PapelUsuario
{
    Administrador = 1,
    Farmaceutico = 2,
    Balconista = 3,
    Financeiro = 4,
    Entregador = 5
}
