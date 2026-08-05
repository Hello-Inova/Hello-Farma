using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.RegistrarDevolucao;

public record ItemDevolucaoInput(Guid ProdutoId, int Quantidade);

/// <summary>
/// Registra uma troca/devolução (total ou parcial) de uma venda do PDV: reestabelece
/// o estoque dos itens devolvidos (FEFO reverso — entram como lote "DEVOLUCAO"),
/// gera um lançamento financeiro de saída e, se a venda tinha cliente vinculado,
/// estorna proporcionalmente o cashback gerado por ela.
/// </summary>
public record RegistrarDevolucaoCommand(Guid VendaId, List<ItemDevolucaoInput> Itens, string? Motivo) : IRequest<Guid>;
