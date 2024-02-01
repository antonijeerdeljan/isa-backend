using AutoMapper;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Dtos.Customer;
using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;
using MassTransit.Initializers;

namespace ISA.Core.Domain.UseCases.User;

public class UserService : BaseService<UserProfileDto, Entities.User.User>
{
    private readonly IIdentityServices _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICompanyAdminRepository _companyAdminRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;


    public UserService(IIdentityServices identityServices, IUserRepository userRepository,ICustomerRepository customerRepository, ICompanyService companyService, IISAUnitOfWork isaUnitOfWork, ICompanyAdminRepository companyAdminRepository, IReservationRepository reservationRepository, IMapper mapper) : base(mapper)
    {
        _identityService = identityServices;
        _userRepository = userRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _customerRepository = customerRepository;
        _companyService = companyService;
        _companyAdminRepository = companyAdminRepository;
        _reservationRepository = reservationRepository;
        _mapper = mapper; 
    }

    public async Task VerifyEmail(string email, string token)
    {
        await _identityService.VerifyEmail(email, token);
    }

    public async Task AddAsync(string email,
                               string password,
                               string firstName,
                               string lastName,
                               string city,
                               string country,
                               DateTime birthDate,
                               string phoneNumber,
                               string? profession,
                               string? companyInfo,
                               string userRole)

    {
        Guid newUserId = Guid.NewGuid();

        await _isaUnitOfWork.StartTransactionAsync();

        Address address = new(country, city);
        Entities.User.User newUser = new(newUserId, firstName, lastName, address, email, phoneNumber, birthDate);


        try
        {
            await _identityService.RegisterUserAsync(newUserId, email, password, userRole);
            await _userRepository.AddAsync(newUser);

            if(profession!= null && companyInfo != null)
            {
                Entities.User.Customer customer = new(newUserId,profession, companyInfo,newUser);
                await _customerRepository.AddAsync(customer);
            }

            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
        catch (Exception ex)
        {
            await _isaUnitOfWork.RollBackAsync();
        }
    }



    public async Task AddCompanyAdminAsync(string email,
                               string password,
                               string firstName,
                               string lastName,
                               string city,
                               string country,
                               DateTime birthDate,
                               string phoneNumber)

    {
        Guid newUserId = Guid.NewGuid();

        await _isaUnitOfWork.StartTransactionAsync();

        Address address = new(country, city);
        Entities.User.User newUser = new(newUserId, firstName, lastName, address, email, phoneNumber, birthDate);


        try
        {
            await _identityService.RegisterUserAsync(newUserId, email, password, "Corpadmin");
            await _userRepository.AddAsync(newUser);

            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
        catch (Exception ex)
        {
            await _isaUnitOfWork.RollBackAsync();
        }
    }




    public async Task CheckForSystemAdmin()
    {
        Address address = new("srbija", "zrenjanin");
        await AddAsync("ftngrupa7@gmail.com", 
                       "Admin123!", 
                       "admin", 
                       "admin", 
                       address.City,
                       address.Country,
                       DateTime.Now.ToUniversalTime(),
                       "123123123",
                       null,
                       null,
                       "Sysadmin");
    }

    public async Task<LoginCookie> LoginAsync(string email, string password)
    {
        return await _identityService.LoginAsync(email, password);
    }

    public async Task<Entities.User.User> GetUserById(Guid id)
    {
        return await _userRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
    }

    public async Task<Entities.User.Customer> GetCustomerById(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
    }


    public async Task<bool> IsRefreshTokenValid(string userId, string refreshToken)
    {
        return await _identityService.VerifyRefreshToken(userId, refreshToken);
    }

    public AuthenticationTokens GenerateNewJWT(string userId, string role)
    {
        return _identityService.GenerateNewJWT(userId, role);
    }

    public async Task UpdateUserAsync(Guid guid, string? name, string? lastname, string? phoneNumber, DateTime? dateOfBirth) 
    {
        Entities.User.User userToUpdate = await GetUserById(guid);
        userToUpdate.Update(name, lastname, phoneNumber, dateOfBirth);
        await _userRepository.SaveAsync();

    }

    public async Task AddNewCorpAdmin(CorpAdminRegistrationDto corpAdmin)

    {
        Guid newUserId = Guid.NewGuid();

        await _isaUnitOfWork.StartTransactionAsync();
        Address address = new(corpAdmin.Country, corpAdmin.City);
        var company = await _companyService.GetCompanyAsync(corpAdmin.CompanyId);
        Entities.User.User newUser = new(newUserId, corpAdmin.Firstname, corpAdmin.Lastname, address, corpAdmin.Email, corpAdmin.PhoneNumber,corpAdmin.DateOfBirth);
        CompanyAdmin newCompanyAdmin = new(newUserId, company.Id);

        try
        {
            await _identityService.RegisterUserAsync(newUserId, corpAdmin.Email, corpAdmin.Password, "Corpadmin");
            await _userRepository.AddAsync(newUser);
            await _companyAdminRepository.AddAsync(newCompanyAdmin);
            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
        catch (Exception ex)
        {
            await _isaUnitOfWork.RollBackAsync();
        }
    }

    public async Task<bool> IsUserIdInCompanyAdmins(Guid userId, Guid companyId)
    {
        return await _companyAdminRepository.CheckIfAdmin(companyId, userId);
    }

    public async Task<IEnumerable<CompanyAdmin>> GetAllCompanyAdmins(Guid adminId, int page)
    {
        var compAdmin = await _companyAdminRepository.GetByIdAsync(adminId);
        return await _companyAdminRepository.GetAllCompanyAdmins(compAdmin.CompanyId, page);
    }

    public async Task<IEnumerable<CustomerProfileDto>> GetAllCompanyCustomers(Guid adminId)
    {
        var compAdmin = await _companyAdminRepository.GetByIdAsync(adminId);
        var reservations = await _reservationRepository.GetAllCompanyCustomers(compAdmin.CompanyId);
        List<Customer> customers = reservations.Select(obj => obj.Customer).ToList();
        //var customers =  _customerRepository.GetAllCompanyCustomers(idList);
        var customerProfiles = customers.Select(customer => _mapper.Map<CustomerProfileDto>(customer));
        return (IEnumerable<CustomerProfileDto>)customerProfiles;
    }

    public async Task GivePenaltyPoints(Guid id,int points)
    {
        var customer = await _customerRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        customer.AddPenaltyPoints(points);
        _customerRepository.Update(customer);
    }

    public async Task RemovePenaltyPoints()
    {
        await _customerRepository.RemovePenaltyPoints();
    }

    public async Task<CompanyAdmin> GetCompanyAdmin(Guid id)
    {
        return await _companyAdminRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException();
    }

    public async Task ChangePassword(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException();
        await _identityService.ChangePasswordAsync(user.Email, currentPassword, newPassword);
    }

    public async Task<int> GetUserPoints(Guid userId)
    {
        var customer = await _customerRepository.GetByIdAsync(userId)
                       ?? throw new KeyNotFoundException("Customer not found with given userId.");

        return customer.Points ?? 0;
    }

    public async Task<int> GetUserPenaltyPoints(Guid userId)
    {
        var customer = await _customerRepository.GetByIdAsync(userId)
                       ?? throw new KeyNotFoundException("Customer not found with given userId.");

        return customer.PenaltyPoints ?? 0;
    }





}
