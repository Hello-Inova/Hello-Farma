using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Billing;
using HelloFarma.Infrastructure.Persistence;

namespace HelloFarma.Infrastructure.Repositories;

public class PlanoRepository(HelloFarmaDbContext context) : EfRepository<Plano>(context), IPlanoRepository
{
}
