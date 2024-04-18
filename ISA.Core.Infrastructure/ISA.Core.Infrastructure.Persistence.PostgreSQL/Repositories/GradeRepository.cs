namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using Microsoft.EntityFrameworkCore;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GradeRepository : GenericRepository<Grade, Guid>, IGradeRepository
    { 
        public GradeRepository(IsaDbContext isaDbContext) : base(isaDbContext)
        {

        }


        public async Task<bool> CheckIfAlreadyExist(Guid userId, Guid companyId)
        {
            var count = await _dbSet.Where(t => t.CustomerUserId == userId && t.CompanyId == companyId).CountAsync();
            return count != 0;
        }

        public async Task<IEnumerable<Grade>> GetAllCompanyGrades(Guid companyId)
        {
            return await _dbSet.Where(t => t.CompanyId == companyId).ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetAllCustomerGrades(Guid userId)
        {
            return await _dbSet.Where(t => t.CustomerUserId == userId).ToListAsync();
        }
    }
}
