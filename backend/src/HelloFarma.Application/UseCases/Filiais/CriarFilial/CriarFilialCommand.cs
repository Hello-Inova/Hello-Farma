using MediatR;

namespace HelloFarma.Application.UseCases.Filiais.CriarFilial;

/// <summary>
/// Cadastra uma nova filial para o tenant autenticado, respeitando o limite de
/// filiais do plano contratado (Assinatura → Plano.LimiteFiliais).
/// </summary>
public record CriarFilialCommand(
    string Nome,
    string? Cnpj,
    string? Endereco,
    string? Cidade,
    string? Uf) : IRequest<Guid>;
