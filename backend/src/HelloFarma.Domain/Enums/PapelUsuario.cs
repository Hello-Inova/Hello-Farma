namespace HelloFarma.Domain.Enums;

/// <summary>
/// Papéis de usuário dentro de um Tenant (farmácia). Controla permissões de acesso
/// aos módulos do ERP. SuperAdmin é reservado para a equipe da Hello Inova, com
/// acesso à Hello Platform (visão administrativa sobre todos os tenants).
/// </summary>
public enum PapelUsuario
{
    Administrador = 1,
    Farmaceutico = 2,
    Balconista = 3,
    Financeiro = 4,
    Entregador = 5,
    SuperAdmin = 99
}
