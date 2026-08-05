using FluentAssertions;
using HelloFarma.Domain.Entities.Auth;
using Xunit;

namespace HelloFarma.UnitTests;

public class RefreshTokenTests
{
    [Fact]
    public void Token_deve_ser_invalido_apos_revogado()
    {
        var token = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(), "token-teste", DateTime.UtcNow.AddDays(1));

        token.EstaValido().Should().BeTrue();

        token.Revogar();

        token.EstaValido().Should().BeFalse();
    }

    [Fact]
    public void Token_deve_ser_invalido_apos_expirar()
    {
        var token = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(), "token-expirado", DateTime.UtcNow.AddSeconds(-1));

        token.EstaValido().Should().BeFalse();
    }
}
