namespace HelloFarma.Application.Interfaces;

public record ResultadoEmissaoFiscal(bool Sucesso, string? ChaveAcesso, string? MotivoRejeicao);

/// <summary>
/// Abstração (Strategy Pattern) para emissão de documentos fiscais. A implementação real
/// (integração com SEFAZ/SAT) fica isolada na camada de Infraestrutura — o domínio e a
/// aplicação nunca dependem diretamente do provedor fiscal escolhido.
/// </summary>
public interface IEmissorFiscal
{
    Task<ResultadoEmissaoFiscal> EmitirAsync(Guid vendaId, decimal valorTotal, CancellationToken ct = default);
}
