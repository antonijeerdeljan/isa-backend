using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService= userService;
    }

    [HttpPost("Register")]
    public async Task RegisterUser([FromBody] RegistrationRequestModel registrationRequestModel)
    => await _userService.AddAsync(registrationRequestModel.Email,
                                   registrationRequestModel.Password,
                                   registrationRequestModel.Firstname,
                                   registrationRequestModel.Lastname,
                                   registrationRequestModel.Address,
                                   registrationRequestModel.DateOfBirth,
                                   registrationRequestModel.PhoneNumber,
                                   IdentityRoles.CUSTOMER);

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestModel loginRequestModel)
    {

        var token = await _userService.LoginAsync(loginRequestModel.Email,
                                   loginRequestModel.Password,
                                   IdentityRoles.CUSTOMER);
        return Ok(token);
    }

};