using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.UseCases.User;
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
                                   registrationRequestModel.PhoneNumber);




};