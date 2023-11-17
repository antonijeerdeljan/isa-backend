
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.UseCases.User;

public class UserService
{
    private readonly IIdentityServices _identityService;
	public UserService(IIdentityServices identityServices)
	{
        _identityService = identityServices;
	}

    public async Task AddAsync(string email, string password, string firstName, string lastName,Address address, DateTime birthDate, string phoneNumber)
    {
        Guid newUserId = Guid.NewGuid();


        Entities.User.User newUser = new(firstName, lastName, address, email, phoneNumber, birthDate) ;

        //await _userRepository.AddAsync(newUser);
        //await UnitOfWork.SaveChangesAsync();

        await _identityService.RegisterAsync(newUserId, email, password);

       
    }
}
