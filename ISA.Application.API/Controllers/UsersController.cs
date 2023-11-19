using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    public UsersController(UserService userService, IHttpContextAccessor contextAccessor)
    {
        _userService= userService;
        _contextAccessor= contextAccessor;
    }

    [HttpPost("Register")]
    public async Task RegisterUser([FromBody] CustomerRegistrationRequestModel registrationRequestModel)
    => await _userService.AddAsync(registrationRequestModel.Email,
                                   registrationRequestModel.Password,
                                   registrationRequestModel.Firstname,
                                   registrationRequestModel.Lastname,
                                   registrationRequestModel.Address,
                                   registrationRequestModel.DateOfBirth,
                                   registrationRequestModel.PhoneNumber,
                                   registrationRequestModel.Profession,
                                   registrationRequestModel.CompanyInfo,
                                   IdentityRoles.CUSTOMER);

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestModel loginRequestModel)
    {
        //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var token = await _userService.LoginAsync(loginRequestModel.Email,
                                   loginRequestModel.Password);


        return Ok(token);
    }

    [HttpPost("EditProfile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Policy = "superAdminPolicy")]
    [Authorize(Policy = "allowAllPolicy")]
    //[Authorize(Policy = "corpAdminPolicy")]
    public async Task<IActionResult> EditProfile([FromBody] EditProfileRequestModel editProfileRequestModel)
    {
        Guid id = Guid.Parse(User.Claims.First(x => x.Type == "id").Value); // TO DO: Make as exstension method

        await _userService.UpdateUserAsync(id,
                                           editProfileRequestModel.Name,
                                           editProfileRequestModel.Lastname,
                                           editProfileRequestModel.PhoneNumber,
                                           editProfileRequestModel.DateOfBirth);


        return Ok();
    }


};