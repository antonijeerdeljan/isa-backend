namespace ISA.Core.Domain.UseCases.Company;

using AutoMapper;
using ceTe.DynamicPDF.PageElements;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using Nest;
using NetTopologySuite.Geometries;
using PolylineEncoder.Net.Models;

public class CompanyService : BaseService<CompanyUpdateDto, Company>, ICompanyService { 
    
    private readonly ICompanyRepository _companyRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpClientService _httpClientService;


    public CompanyService(ICompanyRepository companyRepository, IISAUnitOfWork isaUnitOfWork, IMapper mapper, IHttpClientService httpClientService) : base(mapper)
    {
        _companyRepository = companyRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _mapper = mapper;
        _httpClientService = httpClientService;
    }

    public async Task AddAsync(string name, DateTime startWorkingHour, DateTime endWorkingHour, string description,string country, string city,string street, int number)
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


    public async Task<Company> GetCompanyByAdminIdAsync(Guid adminId)
    {
        return await _companyRepository.GetCompanyByAdminIdAsync(adminId);
    }

    public async Task UpdateAsync(Guid id, string name, string city, string country, string street, int number, string description)
    {
        await _isaUnitOfWork.StartTransactionAsync();
        var updatedCompany = await GetCompanyByAdminIdAsync(id);
        updatedCompany.Address.City = city;
        updatedCompany.Address.Country = country;
        updatedCompany.Address.Street= street;
        updatedCompany.Address.Number = number;
        updatedCompany.Name = name;
        updatedCompany.Description = description;
        _companyRepository.Update(updatedCompany);
        await _isaUnitOfWork.SaveAndCommitChangesAsync();
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

    public async Task<IEnumerable<CompanyProfilesDto>> GetAllCompaniesForUnauthorized(int page)
    {
        var companies = await _companyRepository.GetAllCompanies(page);
        var companyProfiles = companies.Select(company => _mapper.Map<CompanyProfilesDto>(company));
        return companyProfiles;
    }


    public async Task<PolylineEncoder.Net.Models.GeoCoordinate> GetComapnyCoordinate(Guid companyId)
    {
        var comapny = await _companyRepository.GetByIdAsync(companyId) ?? throw new KeyNotFoundException(); 
        var coordinate = await _httpClientService.GetCoordinatesFromAddress(comapny.Address.Street,
                                                           comapny.Address.City,
                                                           comapny.Address.Country,
                                                           comapny.Address.Number.ToString());

        var geoCoordinate = new PolylineEncoder.Net.Models.GeoCoordinate();
        geoCoordinate.Latitude = coordinate.Y;   
        geoCoordinate.Longitude = coordinate.X; 


        return geoCoordinate;
    }


    public async Task<CompanyProfileDto> GetCompanyProfile(Guid id)
    {
        CompanyProfileDto companyDto = new CompanyProfileDto();
        var company = await _companyRepository.GetByIdAsync(id) ?? throw new ArgumentException();
        company.Equipment.RemoveAll(e => e.Quantity <= 0);
        //company.Appointments.RemoveAll(a => a.StartingDateTime <= DateTime.Now);
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
