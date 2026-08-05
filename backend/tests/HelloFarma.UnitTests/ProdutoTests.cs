using FluentAssertions;
using HelloFarma.Domain.Entities.Produtos;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class ProdutoTests
{
    [Fact]
    public void Deve_criar_produto_com_dados_validos()
    {
        var tenantId = Guid.NewGuid();

        var produto = new Produto(tenantId, "Dipirona 500mg", "7891234567890", TipoProduto.Generico, 12.50m, 8.90m);

        produto.Nome.Should().Be("Dipirona 500mg");
        produto.TenantId.Should().Be(tenantId);
        produto.Controlado.Should().BeFalse();
    }
}
