using HelloFarma.Application.DTOs;
using HelloFarma.Application.Interfaces;
using MediatR;

namespace HelloFarma.Application.UseCases.Auditoria.ListarAuditoria;

public class ListarAuditoriaHandler(IAuditoriaRepository auditoriaRepository)
    : IRequestHandler<ListarAuditoriaQuery, IReadOnlyList<LogAuditoriaDto>>
{
    public async Task<IReadOnlyList<LogAuditoriaDto>> Handle(ListarAuditoriaQuery request, CancellationToken ct)
    {
        var logs = await auditoriaRepository.ListarRecentesAsync(request.Quantidade, ct);
        return logs.Select(l => new LogAuditoriaDto(
            l.Id, l.UsuarioId, l.UsuarioNome, l.Acao, l.Sucesso, l.Erro, l.IpAddress, l.CreatedAtUtc)).ToList();
    }
}
