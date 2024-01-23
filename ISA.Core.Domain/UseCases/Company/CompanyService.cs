namespace ISA.Core.Domain.UseCases.Company;

using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
public class CompanyService : BaseService<CompanyUpdateDto, Company>, ICompanyService { 
    
    private readonly ICompanyRepository _companyRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;

    
public CompanyService(ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper) : base(mapper)
    {
        _companyRepository = companyRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(string name, string startWorkingHour, string endWorkingHour, string description,string country, string city)
    {
        

        TimeOnly start;
        TimeOnly end;
        if (TimeOnly.TryParse(startWorkingHour, out start) && TimeOnly.TryParse(endWorkingHour, out end) is false)
        {
            throw new Exception();
        }
        await _isaUnitOfWork.StartTransactionAsync();

        Address address = new(country,city);
        address.Id = Guid.NewGuid();

        Company newCompany = new Company(name, address, description, start, end);
        newCompany.Id = Guid.NewGuid();

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
        nova.Address.Country = company.Country;


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

    public async Task<Company> GetCompanyAsync(Guid id)
    {
        return await _companyRepository.GetByIdAsync(id) ?? throw new ArgumentNullException();
    }

    public async Task<IEnumerable<CompanyProfileDto>> GetAllCompanies(int page)
    {
        var companies = await _companyRepository.GetAllCompanies(page);
        var companyProfiles = companies.Select(company => _mapper.Map<CompanyProfileDto>(company));
        return companyProfiles;
    }


    public async Task<CompanyProfileDto> GetCompanyProfile(Guid id)
    {
        CompanyProfileDto companyDto = new CompanyProfileDto();
        var company = await _companyRepository.GetByIdAsync(id);
        company.Equipment.RemoveAll(e => e.Quantity <= 0);
        return _mapper.Map(company, companyDto);

    }


    public async Task<bool> IsAppointmentInWorkingHours(DateTime start, DateTime end, Guid companyId)
    {
        return await _companyRepository.IsAppointmentInWorkingHours(start, end, companyId);
    }


}
