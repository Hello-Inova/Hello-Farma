using FluentAssertions;
using HelloFarma.Domain.Entities.Vendas;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class VendaTests
{
    [Fact]
    public void Deve_somar_total_ao_adicionar_itens()
    {
        var venda = new Venda(Guid.NewGuid(), Guid.NewGuid(), FormaPagamento.Pix);

        venda.AdicionarItem(Guid.NewGuid(), "Dipirona 500mg", 2, 12.50m);
        venda.AdicionarItem(Guid.NewGuid(), "Soro Fisiológico", 1, 8.00m);

        venda.ValorTotal.Should().Be(33.00m);
        venda.Itens.Should().HaveCount(2);
    }

    [Fact]
    public void Nao_deve_permitir_quantidade_zero_ou_negativa()
    {
        var venda = new Venda(Guid.NewGuid(), Guid.NewGuid(), FormaPagamento.Dinheiro);

        var acao = () => venda.AdicionarItem(Guid.NewGuid(), "Produto X", 0, 10m);

        acao.Should().Throw<InvalidOperationException>();
    }
}
