﻿
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;
using Microsoft.AspNet.Identity;
using System.Runtime.CompilerServices;

namespace ISA.Core.Domain.UseCases.User;

public class UserService
{
    private readonly IIdentityServices _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;

	public UserService(IIdentityServices identityServices, IUserRepository userRepository, IISAUnitOfWork isaUnitOfWork)
	{
        _identityService = identityServices;
        _userRepository = userRepository;
        _isaUnitOfWork = isaUnitOfWork;
	}

    public async Task AddAsync(string email, string password, string firstName, string lastName,Address address, DateTime birthDate, string phoneNumber, string userRole)
    {
        Guid newUserId = Guid.NewGuid();

        await _isaUnitOfWork.StartTransactionAsync();

        Entities.User.User newUser = new(newUserId,firstName, lastName, address, email, phoneNumber, birthDate);

        try
        {
            await _userRepository.AddAsync(newUser);
            await _identityService.RegisterUserAsync(newUserId, email, password, userRole);
            await _isaUnitOfWork.CommitTransactionAsync();

        }
        catch (Exception ex) 
        {

        }

    }

    public async Task<AuthenticationTokens> LoginAsync(string email, string password, string role)
    {
        return await _identityService.LoginAsync(email, password, role);
    }
}
