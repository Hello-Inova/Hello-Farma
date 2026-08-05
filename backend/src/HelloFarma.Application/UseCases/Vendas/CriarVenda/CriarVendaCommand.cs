using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.CriarVenda;

public record ItemVendaInput(Guid ProdutoId, int Quantidade);

/// <summary>
/// Fecha uma venda no PDV: valida produtos, calcula total pelo PMC vigente,
/// baixa estoque via FEFO e persiste a venda com pagamento vinculado.
/// </summary>
public record CriarVendaCommand(List<ItemVendaInput> Itens, int FormaPagamento, Guid? ClienteId) : IRequest<Guid>;
