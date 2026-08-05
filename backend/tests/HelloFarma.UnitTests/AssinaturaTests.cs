using FluentAssertions;
using HelloFarma.Domain.Entities.Billing;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class AssinaturaTests
{
    [Fact]
    public void Assinatura_deve_iniciar_em_trial()
    {
        var assinatura = new Assinatura(Guid.NewGuid(), Guid.NewGuid());

        assinatura.Status.Should().Be(StatusAssinatura.Trial);
    }

    [Fact]
    public void Deve_cancelar_assinatura_registrando_data_fim()
    {
        var assinatura = new Assinatura(Guid.NewGuid(), Guid.NewGuid());

        assinatura.Cancelar();

        assinatura.Status.Should().Be(StatusAssinatura.Cancelada);
        assinatura.FimEm.Should().NotBeNull();
    }
}
