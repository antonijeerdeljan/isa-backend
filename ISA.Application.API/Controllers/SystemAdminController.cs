using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Exceptions.UserExceptions;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SystemAdminController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    public SystemAdminController(UserService userService,
                                 IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "superAdminPolicy")]
    [HttpPost("Register")]
    public async Task RegisterAdmin([FromBody] RegistrationRequestModel registrationRequestModel, string role)
    {

        if (role != IdentityRoles.SYSADMIN || role != IdentityRoles.CORPADMIN)
            await _userService.AddAsync(registrationRequestModel.Email,
                                       registrationRequestModel.Password,
                                       registrationRequestModel.Firstname,
                                       registrationRequestModel.Lastname,
                                       registrationRequestModel.City,
                                       registrationRequestModel.Country,
                                       registrationRequestModel.DateOfBirth,
                                       registrationRequestModel.PhoneNumber,
                                       null,null,
                                       role);
        else
            throw new RoleException("Invalid role detected.");
    }

};
