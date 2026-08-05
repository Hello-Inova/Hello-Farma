using HelloFarma.Application.Interfaces;

namespace HelloFarma.Infrastructure.Services;

/// <summary>
/// Implementação inicial (simulada) do emissor fiscal — gera uma chave de acesso fake.
/// Deve ser substituída por um adaptador real de SEFAZ/SAT quando a integração for feita;
/// como é uma Strategy plugável via DI, a troca não impacta nenhuma outra camada.
/// </summary>
public class EmissorFiscalSimulado : IEmissorFiscal
{
    public Task<ResultadoEmissaoFiscal> EmitirAsync(Guid vendaId, decimal valorTotal, CancellationToken ct = default)
    {
        var chave = $"SIMULADO-{vendaId:N}".ToUpperInvariant();
        return Task.FromResult(new ResultadoEmissaoFiscal(true, chave, null));
    }
}
