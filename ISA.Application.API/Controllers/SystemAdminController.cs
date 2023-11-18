using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Exceptions.UserExceptions;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISA.Application.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Sysadmin")]
[Route("[controller]")]
public class SystemAdminController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    public SystemAdminController(UserService userService, IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
    }

    [HttpPost("Register")]
    public async Task RegisterAdmin([FromBody] RegistrationRequestModel registrationRequestModel, string role)
    {

        if (role != IdentityRoles.SYSADMIN || role != IdentityRoles.CORPADMIN)
            throw new RoleException("Invalid role detected.");
            
        await _userService.AddAsync(registrationRequestModel.Email,
                                       registrationRequestModel.Password,
                                       registrationRequestModel.Firstname,
                                       registrationRequestModel.Lastname,
                                       registrationRequestModel.Address,
                                       registrationRequestModel.DateOfBirth,
                                       registrationRequestModel.PhoneNumber,
                                       role);
    }

};
