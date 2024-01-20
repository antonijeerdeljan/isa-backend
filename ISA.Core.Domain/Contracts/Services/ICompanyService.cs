using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Contracts.Services;

public interface ICompanyService
{
    Task AddAsync(string name, Address address, int startWorkingHour, int endWorkingHour, string description);
    Task UpdateAsync(CompanyUpdateDto company);
    Task<Company> GetCompanyAsync(Guid id);
    Task<CompanyProfileDto> GetCompanyProfile(Guid id);
}
