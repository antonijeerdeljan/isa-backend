using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.UseCases.User;

namespace ISA.Application.API.Startup;

public class SystemAdminCreation
{
    private readonly UserService _userService;
    public SystemAdminCreation(UserService userService)
    {
        _userService = userService;
    }
    public async Task CheckForSystemAdmin()
    {
        var registrationRequestModel = new RegistrationRequestModel
        {
            Email = "admin@gmail.com",
            Password = "Admin123!",
            Firstname = "admin",
            Lastname = "admin",
            DateOfBirth = DateTime.Now,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Country = "srbija",
                City = "zrenjanin",
                Street = "njegoseva",
                Number = 23,
                ZipCode = 23000
            },
            PhoneNumber = "1231231232"
        };


        await _userService.AddAsync(registrationRequestModel.Email,
                                    registrationRequestModel.Password,
                                    registrationRequestModel.Firstname,
                                    registrationRequestModel.Lastname,
                                    registrationRequestModel.Address,
                                    registrationRequestModel.DateOfBirth,
                                    registrationRequestModel.PhoneNumber,
                                    "Sysadmin");
    }
}
