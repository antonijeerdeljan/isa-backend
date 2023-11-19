namespace ISA.Core.Domain.Contracts.Repositories
{
    using ISA.Core.Domain.Entities.Company;
    using System.Threading.Tasks;

    public interface ICompanyRepository
    {
        public Task AddAsync(Company companyToAdd);
        public Task<Company?> GetByIdAsync(Guid id);
    }
}
