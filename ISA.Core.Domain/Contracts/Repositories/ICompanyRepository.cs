namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities;
    using ISA.Core.Domain.Entities.Company;
    using System.Threading.Tasks;

    public interface ICompanyRepository
    {
        Task AddAsync(Company companyToAdd);
        Task<Company?> GetByIdAsync(Guid id);

        void UpdateAndSaveChanges(Company company);

        void Update(Company company);

    }
}
