using FluentAssertions;
using HelloFarma.Domain.Entities.Fiscal;
using HelloFarma.Domain.Enums;
using Xunit;

namespace HelloFarma.UnitTests;

public class DocumentoFiscalTests
{
    [Fact]
    public void Deve_marcar_como_emitido_com_chave_de_acesso()
    {
        var documento = new DocumentoFiscal(Guid.NewGuid(), Guid.NewGuid());

        documento.MarcarEmitido("CHAVE-123");

        documento.Status.Should().Be(StatusDocumentoFiscal.Emitido);
        documento.ChaveAcesso.Should().Be("CHAVE-123");
    }

    [Fact]
    public void Deve_marcar_como_rejeitado_com_motivo()
    {
        var documento = new DocumentoFiscal(Guid.NewGuid(), Guid.NewGuid());

        documento.MarcarRejeitado("CNPJ inválido");

        documento.Status.Should().Be(StatusDocumentoFiscal.Rejeitado);
        documento.MotivoRejeicao.Should().Be("CNPJ inválido");
    }
}
