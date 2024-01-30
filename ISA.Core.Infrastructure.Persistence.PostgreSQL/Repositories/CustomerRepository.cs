using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
{
    private readonly IsaDbContext _isaDbContext;
    public CustomerRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
        _isaDbContext = isaDbContext;
    }

    public async Task<IEnumerable<Customer>> GetAllCompanyCustomers(List<Guid> usersId)
    {
        return (IEnumerable<Customer>)await _dbSet.Where(t => usersId.Contains(t.UserId)).Include(t => t.User).FirstOrDefaultAsync();
    }

    new public async Task<Customer?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Where(t => t.UserId == Id).Include(t => t.User).FirstOrDefaultAsync(); ;
    }

}
