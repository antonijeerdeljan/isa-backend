using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class ContractRepository : GenericRepository<Contract, Guid>, IContractRepository
{
    public ContractRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
    }

    public async Task<bool> CompanyAlreadyHaveContract(Guid companyId)
    {
        return (await _dbSet.Where(c => c.Company.Id == companyId).FirstOrDefaultAsync() == null) ? false : true;
    }

    public async Task<Contract> GetContractByCompanyAsync(Guid companyId)
    {
        return await _dbSet.Include(c=>c.Equipments)
                           .Include(c=>c.Company)
                           .Where(c => c.Company.Id == companyId)
                           .FirstOrDefaultAsync();
    }

    public async Task DeleteContract(Guid id)
    {
        var contract = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
        _dbSet.Remove(contract);
    }

    public async Task<List<Contract>> GetTodaysContract()
    {
        return await _dbSet.Include(c => c.Company)
                   .ThenInclude(company => company.Address)  // Include the Address from Company
                   .Where(c => c.DeliveryDate.Day == DateTime.Now.Day)
                   .ToListAsync();

    }
}
