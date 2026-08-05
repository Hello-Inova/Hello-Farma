using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Compras;
using HelloFarma.Infrastructure.Persistence;

namespace HelloFarma.Infrastructure.Repositories;

public class FornecedorRepository(HelloFarmaDbContext context) : EfRepository<Fornecedor>(context), IFornecedorRepository
{
}
