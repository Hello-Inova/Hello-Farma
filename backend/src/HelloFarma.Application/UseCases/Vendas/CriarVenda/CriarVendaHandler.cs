using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Crm;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Entities.Fiscal;
using HelloFarma.Domain.Entities.Vendas;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.CriarVenda;

public class CriarVendaHandler(
    IProdutoRepository produtoRepository,
    IVendaRepository vendaRepository,
    IBaixaEstoqueService baixaEstoqueService,
    IContaFinanceiraRepository contaFinanceiraRepository,
    IDocumentoFiscalRepository documentoFiscalRepository,
    IClienteRepository clienteRepository,
    ICurrentTenant currentTenant,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CriarVendaCommand, Guid>
{
    /// <summary>Percentual padrão de cashback gerado sobre o valor efetivamente pago pelo cliente.</summary>
    private const decimal PercentualCashback = 0.02m;

    public async Task<Guid> Handle(CriarVendaCommand request, CancellationToken ct)
    {
        if (request.Itens.Count == 0)
            throw new InvalidOperationException("A venda precisa ter ao menos um item.");

        Cliente? cliente = null;
        if (request.ClienteId.HasValue)
        {
            cliente = await clienteRepository.GetByIdAsync(request.ClienteId.Value, ct)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");
        }
        else if (request.CashbackUtilizado > 0)
        {
            throw new InvalidOperationException("É necessário informar um cliente para resgatar cashback.");
        }

        var venda = new Venda(currentTenant.TenantId, currentUser.UsuarioId, (FormaPagamento)request.FormaPagamento, request.ClienteId, request.FilialId);

        foreach (var itemInput in request.Itens)
        {
            var produto = await produtoRepository.GetByIdAsync(itemInput.ProdutoId, ct)
                ?? throw new KeyNotFoundException($"Produto {itemInput.ProdutoId} não encontrado.");

            if (!produto.Ativo)
                throw new InvalidOperationException($"Produto '{produto.Nome}' está inativo.");

            venda.AdicionarItem(produto.Id, produto.Nome, itemInput.Quantidade, produto.Pmc);

            await baixaEstoqueService.BaixarAsync(produto.Id, itemInput.Quantidade, $"Venda {venda.Id}", request.FilialId, ct);
        }

        if (request.CashbackUtilizado > 0 && cliente is not null)
            cliente.ResgatarCashback(request.CashbackUtilizado);

        var cashbackGerado = cliente is not null
            ? Math.Round((venda.ValorTotal - request.CashbackUtilizado) * PercentualCashback, 2)
            : 0m;

        venda.AplicarCashback(request.CashbackUtilizado, cashbackGerado);

        if (cliente is not null)
        {
            cliente.AcumularCashback(cashbackGerado);
            clienteRepository.Update(cliente);
        }

        await vendaRepository.AddAsync(venda, ct);

        var conta = ContaFinanceira.CriarJaPaga(
            currentTenant.TenantId, TipoContaFinanceira.Receber, $"Venda PDV {venda.Id}", venda.ValorPago, "Venda", venda.Id);
        await contaFinanceiraRepository.AddAsync(conta, ct);

        var documentoFiscal = new DocumentoFiscal(currentTenant.TenantId, venda.Id);
        await documentoFiscalRepository.AddAsync(documentoFiscal, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return venda.Id;
    }
}
