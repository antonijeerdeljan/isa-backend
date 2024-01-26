using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
{
    private readonly IsaDbContext _isaDbContext;
    public CustomerRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
        _isaDbContext = isaDbContext;
    }

    new public async Task<Customer?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Where(t => t.UserId == Id).Include(t => t.User).FirstOrDefaultAsync(); ;
    }

}
