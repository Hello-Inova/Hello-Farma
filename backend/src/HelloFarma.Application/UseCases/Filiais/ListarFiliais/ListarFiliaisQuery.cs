using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.ListarFiliais;

public record ListarFiliaisQuery : IRequest<IReadOnlyList<FilialDto>>;
