using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Auditoria.ListarAuditoria;

public record ListarAuditoriaQuery(int Quantidade = 200) : IRequest<IReadOnlyList<LogAuditoriaDto>>;
