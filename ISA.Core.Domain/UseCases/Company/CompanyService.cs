namespace ISA.Core.Domain.UseCases.Company;

using AutoMapper;
using ceTe.DynamicPDF.PageElements;
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

    public async Task AddAsync(string name, TimeOnly startWorkingHour, TimeOnly endWorkingHour, string description,string country, string city,string street, int number)
    {
        

        
        await _isaUnitOfWork.StartTransactionAsync();

        Address address = new(country,city,street,number);
        address.Id = Guid.NewGuid();

        Company newCompany = new Company(name, address, description, startWorkingHour, endWorkingHour);
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
        var company = await _companyRepository.GetByIdAsync(id) ?? throw new ArgumentException();
        company.Equipment.RemoveAll(e => e.Quantity <= 0);
        return _mapper.Map(company, companyDto);

    }

    public async Task<List<Guid>> GetCompanyAdmins(Guid compnayId)
    {
        return await _companyRepository.GetAdmins(compnayId);
    }




    public async Task<bool> IsAppointmentInWorkingHours(DateTime start, DateTime end, Guid id)
    {
        var company = await GetCompanyAsync(id);
        if (company == null)
        {
            throw new ArgumentException();
        }

        TimeSpan companyStart = company.StartingWorkingHour.ToTimeSpan();
        TimeSpan companyEnd = company.EndWorkingHour.ToTimeSpan();

        bool isWithinWorkingHours = start.TimeOfDay >= companyStart && end.TimeOfDay <= companyEnd;

        bool isAfterCurrentTimePlusOneHour = start >= DateTime.UtcNow.AddHours(1);

        bool isSameDateAndWeekday = start.Date == end.Date &&
                                    start.Date.DayOfWeek != DayOfWeek.Sunday &&
                                    start.Date.DayOfWeek != DayOfWeek.Saturday;

        bool isEndTimeAfterStartTime = end >= start;

        return isWithinWorkingHours && isAfterCurrentTimePlusOneHour &&
               isSameDateAndWeekday && isEndTimeAfterStartTime;
    }



}
