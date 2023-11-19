namespace ISA.Application.API.Controllers
{
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Sysadmin")]
        [HttpPost("Register")]
        public async Task RegisterCompany([FromBody] Company company)
        => await _companyService.AddAsync(company.Name,
                                       company.Address,
                                       company.Description,
                                       company.AverageGrade,
                                       company.Appointments,
                                       company.Admins);


    }
}
