namespace ISA.Application.API.Controllers
{
    using ISA.Application.API.Models.Requests;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.UseCases.Company;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        public async Task UpdateCompany([FromBody] CompanyUpdateDto company)
        => await _companyService.UpdateAsync(company);


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



    }
}
