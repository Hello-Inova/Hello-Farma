using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Auditoria;
using MediatR;

namespace HelloFarma.Application.Behaviors;

/// <summary>
/// Intercepta toda ação de escrita (qualquer request cujo nome termine em "Command")
/// executada por um usuário autenticado e grava um log de auditoria — quem fez o quê,
/// quando e se teve sucesso. Consultas (Query) não são auditadas. O log só é gravado
/// quando a ação termina sem exceção, para não arriscar reaproveitar um DbContext
/// que pode ter ficado em estado inconsistente após uma falha.
/// </summary>
public class AuditoriaBehavior<TRequest, TResponse>(
    IAuditoriaRepository auditoriaRepository,
    IUnitOfWork unitOfWork,
    ICurrentTenant currentTenant,
    ICurrentUser currentUser,
    IRequestContext requestContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var nomeAcao = typeof(TRequest).Name;
        var ehComando = nomeAcao.EndsWith("Command", StringComparison.Ordinal);

        if (!ehComando || currentUser.UsuarioId == Guid.Empty)
            return await next();

        var response = await next();

        try
        {
            var log = new LogAuditoria(currentTenant.TenantId, currentUser.UsuarioId, currentUser.Nome, nomeAcao, true, null, requestContext.IpAddress);
            await auditoriaRepository.AddAsync(log, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
        catch
        {
            // Auditoria nunca deve derrubar a operação principal, que já foi concluída com sucesso.
        }

        return response;
    }
}
