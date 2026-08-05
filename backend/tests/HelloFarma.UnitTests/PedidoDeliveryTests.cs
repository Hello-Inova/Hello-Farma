using FluentAssertions;
using HelloFarma.Domain.Entities.Delivery;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class PedidoDeliveryTests
{
    [Fact]
    public void Deve_seguir_o_fluxo_completo_ate_avaliado()
    {
        var pedido = new PedidoDelivery(Guid.NewGuid(), Guid.NewGuid(), "Rua das Flores, 123");

        pedido.AvancarPara(StatusPedidoDelivery.Separacao);
        pedido.AvancarPara(StatusPedidoDelivery.Expedicao);
        pedido.AvancarPara(StatusPedidoDelivery.EmRota);
        pedido.AvancarPara(StatusPedidoDelivery.Entregue);
        pedido.Avaliar(5);

        pedido.Status.Should().Be(StatusPedidoDelivery.Avaliado);
        pedido.AvaliacaoNota.Should().Be(5);
        pedido.EntregueEmUtc.Should().NotBeNull();
    }

    [Fact]
    public void Nao_deve_pular_etapas_do_fluxo()
    {
        var pedido = new PedidoDelivery(Guid.NewGuid(), Guid.NewGuid(), "Rua X, 1");

        var acao = () => pedido.AvancarPara(StatusPedidoDelivery.Entregue);

        acao.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Nao_deve_avaliar_pedido_que_ainda_nao_foi_entregue()
    {
        var pedido = new PedidoDelivery(Guid.NewGuid(), Guid.NewGuid(), "Rua Y, 2");

        var acao = () => pedido.Avaliar(4);

        acao.Should().Throw<InvalidOperationException>();
    }
}
