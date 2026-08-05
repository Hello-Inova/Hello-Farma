using FluentAssertions;
using HelloFarma.Domain.Entities.Estoque;
using Xunit;

namespace HelloFarma.UnitTests;

public class LoteTests
{
    [Fact]
    public void Deve_lancar_erro_ao_baixar_mais_que_o_disponivel()
    {
        var lote = new Lote(Guid.NewGuid(), Guid.NewGuid(), "L001", DateOnly.FromDateTime(DateTime.Today.AddMonths(6)), 10);

        var acao = () => lote.Baixar(20);

        acao.Should().Throw<InvalidOperationException>().WithMessage("*insuficiente*");
    }

    [Fact]
    public void Deve_identificar_lote_proximo_do_vencimento()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var lote = new Lote(Guid.NewGuid(), Guid.NewGuid(), "L002", hoje.AddDays(30), 5);

        lote.ProximoDoVencimento(hoje, diasAlerta: 90).Should().BeTrue();
        lote.ProximoDoVencimento(hoje, diasAlerta: 10).Should().BeFalse();
    }

    [Fact]
    public void Lote_vencido_nao_deve_ser_considerado_proximo_do_vencimento()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var lote = new Lote(Guid.NewGuid(), Guid.NewGuid(), "L003", hoje.AddDays(-5), 5);

        lote.VencidoEm(hoje).Should().BeTrue();
        lote.ProximoDoVencimento(hoje).Should().BeFalse();
    }
}
