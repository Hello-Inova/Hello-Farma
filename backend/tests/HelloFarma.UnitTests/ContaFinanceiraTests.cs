using FluentAssertions;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class ContaFinanceiraTests
{
    [Fact]
    public void Conta_criada_ja_paga_deve_ter_status_paga_e_data_de_pagamento()
    {
        var conta = ContaFinanceira.CriarJaPaga(Guid.NewGuid(), TipoContaFinanceira.Receber, "Venda PDV", 100m, "Venda", Guid.NewGuid());

        conta.Status.Should().Be(StatusContaFinanceira.Paga);
        conta.PagaEmUtc.Should().NotBeNull();
    }

    [Fact]
    public void Conta_pendente_deve_mudar_para_paga_ao_baixar()
    {
        var conta = new ContaFinanceira(Guid.NewGuid(), TipoContaFinanceira.Pagar, "Compra fornecedor X", 500m, DateOnly.FromDateTime(DateTime.Today.AddDays(30)));

        conta.Status.Should().Be(StatusContaFinanceira.Pendente);

        conta.MarcarComoPaga();

        conta.Status.Should().Be(StatusContaFinanceira.Paga);
    }
}
