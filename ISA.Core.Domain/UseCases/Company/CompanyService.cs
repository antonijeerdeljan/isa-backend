namespace ISA.Core.Domain.UseCases.Company
{
    using ISA.Core.Domain.Contracts;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using System.Xml.Linq;

    public class CompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IISAUnitOfWork _isaUnitOfWork;

        public CompanyService(ICompanyRepository companyRepository,IUserRepository userRepository, IISAUnitOfWork isaUnitOfWork)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _isaUnitOfWork = isaUnitOfWork;
        }

        public async Task AddAsync(string name, Address address, string description, double averageGrade, List<Appointment> appointments, List<User> admins)
        {
            Guid newCompanyId = Guid.NewGuid();

            await _isaUnitOfWork.StartTransactionAsync();

            Entities.Company.Company newCompany = new Company(newCompanyId, name, address, description, averageGrade, appointments, admins);
           

            try
            {

            
                await _companyRepository.AddAsync(newCompany);
                await _isaUnitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {

            }

        }
    }
}
