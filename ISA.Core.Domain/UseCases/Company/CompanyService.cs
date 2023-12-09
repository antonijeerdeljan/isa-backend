namespace ISA.Core.Domain.UseCases.Company
{
    using AutoMapper;
    using ISA.Core.Domain.Contracts;
    using ISA.Core.Domain.Contracts.Repositories;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using System.Xml.Linq;

    public class CompanyService : BaseService<CompanyUpdateDto,Company>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IISAUnitOfWork _isaUnitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper) : base(mapper)
        {
            _companyRepository = companyRepository;
            _isaUnitOfWork = isaUnitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(string name, Address address, string description, double averageGrade, List<Appointment> appointments, List<User> admins)
        {
            //Guid newCompanyId = Guid.NewGuid();

            await _isaUnitOfWork.StartTransactionAsync();

            Company newCompany = new Company(name, address, description, averageGrade, appointments, admins);

            try
            { 
                await _companyRepository.AddAsync(newCompany);
                await _isaUnitOfWork.SaveAndCommitChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public async Task UpdateAsync(CompanyUpdateDto company)
        {
            var nova = await _companyRepository.GetByIdAsync(company.Id);
            _mapper.Map(company, nova);
            nova.Address.City = company.City;
            nova.Address.Country= company.Country;


            await _isaUnitOfWork.StartTransactionAsync();

            try
            {
                
                _companyRepository.Update(nova);
                //_addressRepository.Update(nova.Address);
                await _isaUnitOfWork.SaveAndCommitChangesAsync();

            }
            catch (Exception ex)
            {

            }

        }
    }
}
