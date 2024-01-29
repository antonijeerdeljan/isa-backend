using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Company;

namespace ISA.Core.Domain.Contracts.Services;

public interface ICompanyService
{
    Task AddAsync(string name, TimeOnly startWorkingHour, TimeOnly endWorkingHour, string description, string country, string city,string street,int number);
    Task UpdateAsync(CompanyUpdateDto company);
    Task<Company> GetCompanyAsync(Guid id);
    Task<CompanyProfileDto> GetCompanyProfile(Guid id);
    Task<IEnumerable<CompanyProfileDto>> GetAllCompanies(int page);

    Task<bool> IsAppointmentInWorkingHours(DateTime start, DateTime end, Guid companyId);
}
