using FluentAssertions;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class ProdutoAtualizacaoTests
{
    [Fact]
    public void Deve_atualizar_preco_do_produto()
    {
        var produto = new Produto(Guid.NewGuid(), "Paracetamol 750mg", "7891111111111", TipoProduto.Generico, 20m, 15m);

        produto.AtualizarPreco(22.90m, 16.50m);

        produto.Pmc.Should().Be(22.90m);
        produto.Pf.Should().Be(16.50m);
    }

    [Fact]
    public void Deve_desativar_produto()
    {
        var produto = new Produto(Guid.NewGuid(), "Ibuprofeno 400mg", "7892222222222", TipoProduto.Similar, 18m, 12m);

        produto.Desativar();

        produto.Ativo.Should().BeFalse();
    }
}
