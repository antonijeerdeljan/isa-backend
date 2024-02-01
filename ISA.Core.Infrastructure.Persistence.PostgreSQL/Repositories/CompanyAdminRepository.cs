using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class CompanyAdminRepository : GenericRepository<CompanyAdmin, Guid>, ICompanyAdminRepository
{
    public CompanyAdminRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }

    new public async Task<CompanyAdmin?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Include(c => c.User).Where(t => t.UserId == Id).FirstOrDefaultAsync(); 
    }

    public async Task<IEnumerable<CompanyAdmin>> GetAllCompanyAdmins(Guid id, int page)
    {
        return await _dbSet.Where(c => c.CompanyId == id).Include(c => c.User).Include(c => c.User.Address).GetPaged(page).ToListAsync(); 
    }
    public async Task<bool> CheckIfAdmin(Guid companyId, Guid userId)
    {
        return await _dbSet.AnyAsync(c => c.UserId == userId && c.CompanyId == companyId);
    }

}
