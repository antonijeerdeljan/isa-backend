namespace ISA.Application.API.Controllers
{
    using ISA.Application.API.Models.Requests;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.User;
    using ISA.Core.Domain.UseCases.Company;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly GradeService _gradeService;
        private readonly IHttpContextAccessor _contextAccessor;

        public GradeController(GradeService gradeService, IHttpContextAccessor contextAccessor)
        {
            _gradeService = gradeService;
            _contextAccessor = contextAccessor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Authorize(Policy = "customerPolicy")]
        public async Task AddGrade([FromBody] GradeRequestModel grade)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _gradeService.AddAsync(grade.CompanyId, userId, grade.Rate, grade.Reason, grade.Text);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Authorize(Policy = "customerPolicy")]
        public async Task UpdateGrade([FromBody] UpdateGradeRequest grade)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _gradeService.Update(userId,grade.Id,grade.Rate, grade.Reason, grade.Text);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CompanyRates/{companyId}")]
        [Authorize(Policy = "customerPolicy")]
        public async Task<IEnumerable<GradeDto>> CompanyRates([FromRoute] Guid companyId)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            return await _gradeService.GetAllCompanyGrades(companyId);
        }
    }
}
