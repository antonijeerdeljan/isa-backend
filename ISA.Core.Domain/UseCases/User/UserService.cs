using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Dtos;
using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.UseCases.User;

public class UserService
{
    private readonly IIdentityServices _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICompanyAdminRepository _companyAdminRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;
    private readonly ICompanyService _companyService;


	public UserService(IIdentityServices identityServices, IUserRepository userRepository,ICustomerRepository customerRepository, ICompanyService companyService, IISAUnitOfWork isaUnitOfWork, ICompanyAdminRepository companyAdminRepository)
	{
        _identityService = identityServices;
        _userRepository = userRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _customerRepository = customerRepository;
        _companyService = companyService;
        _companyAdminRepository = companyAdminRepository;
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
        CompanyAdmin newCompanyAdmin = new(newUser, company);

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

    public async Task<IEnumerable<CompanyAdmin>> GetAllCompanyAdmins(Guid id, int page)
    {
        return await _companyAdminRepository.GetAllCompanyAdmins(id, page);
    }
}
