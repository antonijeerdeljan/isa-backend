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
    private readonly ICustomerRepository _customerRepository;
    private readonly IISAUnitOfWork _isaUnitOfWork;

	public UserService(IIdentityServices identityServices, IUserRepository userRepository,ICustomerRepository customerRepository, IISAUnitOfWork isaUnitOfWork)
	{
        _identityService = identityServices;
        _userRepository = userRepository;
        _isaUnitOfWork = isaUnitOfWork;
        _customerRepository = customerRepository;
	}

    public async Task AddAsync(string email,
                               string password,
                               string firstName,
                               string lastName,
                               Address address,
                               DateTime birthDate,
                               string phoneNumber,
                               string? profession,
                               string? companyInfo,
                               string userRole)
    {
        Guid newUserId = Guid.NewGuid();

        await _isaUnitOfWork.StartTransactionAsync();
        Entities.User.User newUser = new(newUserId,firstName, lastName, address, email, phoneNumber, birthDate);

        try
        {
            await _userRepository.AddAsync(newUser);

            if(profession!= null && companyInfo != null)
            {
                Entities.User.Customer customer = new(newUserId,profession, companyInfo,newUser);
                await _customerRepository.AddAsync(customer);
            }

            await _identityService.RegisterUserAsync(newUserId, email, password, userRole);
            await _isaUnitOfWork.SaveAndCommitChangesAsync();
        }
        catch (Exception ex) 
        {

        }

    }
    public async Task CheckForSystemAdmin()
    {
        Address address = new Address
        {
            Id = Guid.NewGuid(),
            Country = "srbija",
            City = "zrenjanin"
        };
        await AddAsync("admin@gmail.com", 
                       "Admin123!", 
                       "admin", 
                       "admin", 
                       address,DateTime.Now.ToUniversalTime(),
                       "123123123",
                       null,
                       null,
                       "Sysadmin");
    }

    public async Task<AuthenticationTokens> LoginAsync(string email, string password)
    {
        return await _identityService.LoginAsync(email, password);
    }

    public async Task<Entities.User.User> GetUserById(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task UpdateUserAsync(Guid guid, string? name, string? lastname, string? phoneNumber, DateTime? dateOfBirth) 
    {
        Entities.User.User userToUpdate = await GetUserById(guid);
        userToUpdate.Update(name, lastname, phoneNumber, dateOfBirth);
        await _userRepository.SaveAsync();

    }
}
