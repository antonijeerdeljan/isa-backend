using ISA.Core.Domain.Entities.Company;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface ICompanyRepository
{
    Task AddAsync(Company companyToAdd);
    Task<Company?> GetByIdAsync(Guid id);
    void UpdateAndSaveChanges(Company company);
    void Update(Company company);
    bool Exist(Guid id);
    Task<IEnumerable<Company>> GetAllCompanies(int page);

    Task<Company> GetCompanyByAdminIdAsync(Guid adminId);
    Task<List<Guid>> GetAdmins(Guid companyId);
}
