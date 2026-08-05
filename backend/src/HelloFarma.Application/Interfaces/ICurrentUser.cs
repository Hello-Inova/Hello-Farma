namespace HelloFarma.Application.Interfaces;

/// <summary>
/// Expõe o usuário autenticado na requisição atual (extraído do JWT).
/// </summary>
public interface ICurrentUser
{
    Guid UsuarioId { get; }
    string? Nome { get; }
    string? Papel { get; }
}
