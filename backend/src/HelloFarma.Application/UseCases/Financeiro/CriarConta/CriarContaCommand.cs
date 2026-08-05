using MediatR;

namespace HelloFarma.Application.UseCases.Financeiro.CriarConta;

public record CriarContaCommand(int Tipo, string Descricao, decimal Valor, DateOnly DataVencimento) : IRequest<Guid>;
