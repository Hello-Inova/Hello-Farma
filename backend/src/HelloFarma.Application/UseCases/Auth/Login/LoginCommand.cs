using MediatR;

namespace HelloFarma.Application.UseCases.Auth.Login;

public record LoginCommand(string Email, string Senha) : IRequest<LoginResult>;

public record LoginResult(string AccessToken, DateTime ExpiraEmUtc, string RefreshToken, string NomeUsuario, string Papel, Guid TenantId);
