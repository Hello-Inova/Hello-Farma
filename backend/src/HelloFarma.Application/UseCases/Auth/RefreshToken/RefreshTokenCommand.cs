using MediatR;

namespace HelloFarma.Application.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResult>;

public record RefreshTokenResult(string AccessToken, DateTime ExpiraEmUtc, string RefreshToken);
