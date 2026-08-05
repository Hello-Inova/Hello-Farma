using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Estoque.ListarLotesProximosVencimento;

public record ListarLotesProximosVencimentoQuery(int DiasAlerta = 90) : IRequest<IReadOnlyList<LoteDto>>;
