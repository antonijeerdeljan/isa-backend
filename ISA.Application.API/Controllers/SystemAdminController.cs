using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Entities.LoyaltyProgram;
using ISA.Core.Domain.Exceptions.UserExceptions;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Formats.Asn1;
using System.Xml.Linq;

namespace ISA.Application.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SystemAdminController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILoyaltyProgramService _loyaltyProgramService;
    public SystemAdminController(UserService userService,
                                 IHttpContextAccessor contextAccessor,
                                 ILoyaltyProgramService loyaltyProgramService)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _loyaltyProgramService = loyaltyProgramService;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "superAdminPolicy")]
    [HttpPost("Register")]
    public async Task RegisterSystemAdmin([FromBody] RegistrationRequestModel registrationRequestModel)
    {
            await _userService.AddAsync(registrationRequestModel.Email,
                                       registrationRequestModel.Password,
                                       registrationRequestModel.Firstname,
                                       registrationRequestModel.Lastname,
                                       registrationRequestModel.City,
                                       registrationRequestModel.Country,
                                       registrationRequestModel.DateOfBirth,
                                       registrationRequestModel.PhoneNumber,
                                       null,null,
                                       "Sysadmin");
    }

    /*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "superAdminPolicy")]
    [HttpPost("Register")]
    public async Task RegisterCompanyAdmin([FromBody] RegistrationRequestModel registrationRequestModel, Guid comapnyId)
    {

            await _userService.AddCompanyAdminAsync(registrationRequestModel.Email,
                                       registrationRequestModel.Password,
                                       registrationRequestModel.Firstname,
                                       registrationRequestModel.Lastname,
                                       registrationRequestModel.City,
                                       registrationRequestModel.Country,
                                       registrationRequestModel.DateOfBirth,
                                       registrationRequestModel.PhoneNumber);
    }*/


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "superAdminPolicy")]
    [HttpPost("DefineLoyaltyProgram")]

    public async Task CreateLoyaltyProgram([FromBody] LoyaltyProgramRequest loyaltyProgramReqeust)
    {
        LoyaltyProgram loyaltyProgram = new(loyaltyProgramReqeust.Name,
                                            loyaltyProgramReqeust.NewPoints, 
                                            loyaltyProgramReqeust.MinCategoryThresholds, 
                                            loyaltyProgramReqeust.MaxPenaltyPoints,
                                            loyaltyProgramReqeust.MaxPenaltyPoints,
                                            loyaltyProgramReqeust.CategoryDiscounts);

        await _loyaltyProgramService.CreateLoyaltyProgramAsync(loyaltyProgram);
    }

};
