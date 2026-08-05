using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Estoque;
using HelloFarma.Infrastructure.Persistence;

namespace HelloFarma.Infrastructure.Repositories;

public class MovimentacaoEstoqueRepository(HelloFarmaDbContext context)
    : EfRepository<MovimentacaoEstoque>(context), IMovimentacaoEstoqueRepository
{
}
