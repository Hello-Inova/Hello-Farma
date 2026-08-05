using HelloFarma.Application.Interfaces;

namespace HelloFarma.Infrastructure.Security;

/// <summary>
/// Implementação com BCrypt (work factor 12), conforme diretriz de segurança do Hello Farma.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha, WorkFactor);

    public bool Verificar(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}
