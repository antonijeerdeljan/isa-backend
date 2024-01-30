namespace ISA.Application.API.Controllers;

using ISA.Application.API.Models.Requests;
using ISA.Core.Domain.Dtos.Company;
using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.UseCases.Company;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _companyService;
    private readonly IHttpContextAccessor _contextAccessor;
    public CompanyController(CompanyService companyService, IHttpContextAccessor contextAccessor)
    {
        _companyService = companyService;
        _contextAccessor = contextAccessor;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Authorize(Policy = "superAdminPolicy")]
    public async Task RegisterCompany([FromBody] CompanyRegisterModel company)
    => await _companyService.AddAsync(company.Name,
                                      company.StartingWorkingHour,
                                      company.EndWorkingHour,
                                      company.Description,
                                      company.Country,
                                      company.City,
                                      company.Street,
                                      company.Number);



    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut]
    [Authorize(Policy = "corpAdminPolicy")]
    public async Task UpdateCompany([FromBody] CompanyUpdateRequest company)
    {
        Guid id = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
        await _companyService.UpdateAsync(id, 
                                          company.Name, 
                                          company.City, 
                                          company.Country, 
                                          company.Street, 
                                          company.Number, 
                                          company.Description);
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    [Authorize(Policy = "corpAdminPolicy")]
    public ActionResult<Company> GetCompany([FromQuery] Guid id)
    {
        return _companyService.GetCompanyAsync(id).Result;
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("Profile/{id}")]
    [Authorize(Policy = "allowAllPolicy")]
    public ActionResult<CompanyProfileDto> GetCompanyProfile([FromRoute] Guid id)
    {
        return _companyService.GetCompanyProfile(id).Result;
    }

    [HttpGet("{page}")]
    public async Task<IEnumerable<CompanyProfileDto>> GettAllCompanies(int page)
    {
        return await _companyService.GetAllCompanies(page);
    }

    [HttpGet("getCompnayLocation/{id}")]
    public async Task<Coordinate> GetCompanyCoordinate(Guid companyId)
    {
        return await _companyService.GetComapnyCoordinate(companyId);
    }

}
