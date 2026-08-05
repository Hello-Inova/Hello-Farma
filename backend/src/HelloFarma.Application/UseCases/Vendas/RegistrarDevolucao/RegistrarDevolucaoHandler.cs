using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Crm;
using HelloFarma.Domain.Entities.Financeiro;
using HelloFarma.Domain.Entities.Vendas;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Vendas.RegistrarDevolucao;

public class RegistrarDevolucaoHandler(
    IVendaRepository vendaRepository,
    IDevolucaoRepository devolucaoRepository,
    IClienteRepository clienteRepository,
    IEntradaEstoqueService entradaEstoqueService,
    IContaFinanceiraRepository contaFinanceiraRepository,
    ICurrentTenant currentTenant,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarDevolucaoCommand, Guid>
{
    public async Task<Guid> Handle(RegistrarDevolucaoCommand request, CancellationToken ct)
    {
        if (request.Itens.Count == 0)
            throw new InvalidOperationException("A devolução precisa ter ao menos um item.");

        var venda = await vendaRepository.ObterComItensAsync(request.VendaId, ct)
            ?? throw new KeyNotFoundException("Venda não encontrada.");

        if (venda.Status == StatusVenda.Cancelada || venda.Status == StatusVenda.Devolvida)
            throw new InvalidOperationException("Esta venda já está cancelada/totalmente devolvida.");

        var devolucoesAnteriores = await devolucaoRepository.ListarPorVendaAsync(venda.Id, ct);
        var jaDevolvidoPorProduto = devolucoesAnteriores
            .SelectMany(d => d.Itens)
            .GroupBy(i => i.ProdutoId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantidade));

        var devolucao = new Devolucao(currentTenant.TenantId, venda.Id, request.Motivo);

        foreach (var itemInput in request.Itens)
        {
            var itemVenda = venda.Itens.FirstOrDefault(i => i.ProdutoId == itemInput.ProdutoId)
                ?? throw new InvalidOperationException($"Produto {itemInput.ProdutoId} não faz parte desta venda.");

            var jaDevolvido = jaDevolvidoPorProduto.GetValueOrDefault(itemInput.ProdutoId);
            if (itemInput.Quantidade + jaDevolvido > itemVenda.Quantidade)
                throw new InvalidOperationException($"Quantidade a devolver de '{itemVenda.ProdutoNome}' excede o que foi vendido.");

            devolucao.AdicionarItem(itemVenda.ProdutoId, itemVenda.ProdutoNome, itemInput.Quantidade, itemVenda.PrecoUnitario);

            await entradaEstoqueService.EntrarAsync(
                itemVenda.ProdutoId, "DEVOLUCAO", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2)), itemInput.Quantidade,
                null, $"Devolução da venda {venda.Id}", venda.FilialId, ct);
        }

        // Estorno de cashback proporcional ao valor devolvido, se a venda gerou cashback para um cliente.
        Cliente? cliente = null;
        if (venda.ClienteId.HasValue && venda.CashbackGerado > 0 && venda.ValorTotal > 0)
        {
            cliente = await clienteRepository.GetByIdAsync(venda.ClienteId.Value, ct);
            if (cliente is not null)
            {
                var proporcao = devolucao.ValorTotal / venda.ValorTotal;
                var estorno = Math.Round(venda.CashbackGerado * proporcao, 2);
                cliente.EstornarCashback(estorno);
                devolucao.DefinirCashbackEstornado(estorno);
                clienteRepository.Update(cliente);
            }
        }

        await devolucaoRepository.AddAsync(devolucao, ct);

        var totalDevolvidoAcumulado = jaDevolvidoPorProduto.Values.Sum() + request.Itens.Sum(i => i.Quantidade);
        var totalVendido = venda.Itens.Sum(i => i.Quantidade);
        if (totalDevolvidoAcumulado >= totalVendido)
            venda.MarcarDevolvida();
        else
            venda.MarcarParcialmenteDevolvida();

        vendaRepository.Update(venda);

        var conta = ContaFinanceira.CriarJaPaga(
            currentTenant.TenantId, TipoContaFinanceira.Pagar, $"Devolução da venda {venda.Id}", devolucao.ValorTotal, "Devolucao", devolucao.Id);
        await contaFinanceiraRepository.AddAsync(conta, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return devolucao.Id;
    }
}
