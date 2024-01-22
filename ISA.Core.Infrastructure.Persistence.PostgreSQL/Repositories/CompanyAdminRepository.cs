using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CompanyAdminRepository : GenericRepository<CompanyAdmin, Guid>, ICompanyAdminRepository
{
    public CompanyAdminRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }

    public async Task<bool> CheckIfAdmin(Guid companyId, Guid userId)
    {
        return await _dbSet.AnyAsync(c => c.UserId == userId && c.Company.Id == companyId);
    }

}
