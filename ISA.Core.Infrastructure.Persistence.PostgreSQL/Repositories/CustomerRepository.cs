using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
{
    private readonly IsaDbContext _isaDbContext;
    public CustomerRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
        _isaDbContext = isaDbContext;
    }

}
