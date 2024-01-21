using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;

namespace ISA.Core.Domain.Contracts.Services;

public interface ICompanyService
{
    Task AddAsync(string name, int startWorkingHour, int endWorkingHour, string description, string country, string city);
    Task UpdateAsync(CompanyUpdateDto company);
    Task<Company> GetCompanyAsync(Guid id);
    Task<CompanyProfileDto> GetCompanyProfile(Guid id);
    Task<IEnumerable<CompanyProfileDto>> GetAllCompanies(int page);
}
