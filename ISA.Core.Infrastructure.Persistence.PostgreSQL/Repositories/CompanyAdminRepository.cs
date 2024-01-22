using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CompanyAdminRepository : GenericRepository<CompanyAdmin, Guid>, ICompanyAdminRepository
{
    public CompanyAdminRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }

    public async Task<IEnumerable<CompanyAdmin>> GetAllCompanyAdmins(Guid id, int page)
    {
        return await _dbSet.GetPaged(page).Where(c => c.Company.Id == id).Include(c => c.User).Include(c => c.User.Address).ToListAsync(); 
    }
}
