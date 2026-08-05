using FluentAssertions;
using HelloFarma.Domain.Entities.Compras;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class PedidoCompraTests
{
    [Fact]
    public void Deve_seguir_o_fluxo_cotacao_pedido_recebimento()
    {
        var pedido = new PedidoCompra(Guid.NewGuid(), Guid.NewGuid());
        pedido.Status.Should().Be(StatusPedidoCompra.Cotacao);

        pedido.AdicionarItem(Guid.NewGuid(), "Dipirona 500mg", 100, 8.50m, "L100", DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        pedido.ConfirmarPedido();
        pedido.Status.Should().Be(StatusPedidoCompra.PedidoRealizado);
        pedido.ValorTotal.Should().Be(850.00m);

        pedido.Receber();
        pedido.Status.Should().Be(StatusPedidoCompra.Recebido);
        pedido.RecebidoEmUtc.Should().NotBeNull();
    }

    [Fact]
    public void Nao_deve_confirmar_pedido_sem_itens()
    {
        var pedido = new PedidoCompra(Guid.NewGuid(), Guid.NewGuid());

        var acao = () => pedido.ConfirmarPedido();

        acao.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Nao_deve_receber_pedido_ainda_em_cotacao()
    {
        var pedido = new PedidoCompra(Guid.NewGuid(), Guid.NewGuid());

        var acao = () => pedido.Receber();

        acao.Should().Throw<InvalidOperationException>();
    }
}
