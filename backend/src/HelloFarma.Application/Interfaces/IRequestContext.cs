namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Expõe metadados da requisição HTTP atual (ex.: IP de origem) sem acoplar a camada
/// de aplicação a tipos do ASP.NET Core.
/// </summary>
public interface IRequestContext
{
    string? IpAddress { get; }
}
