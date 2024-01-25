namespace ISA.Application.API.Controllers
{
    using ISA.Core.Domain.Contracts.Services;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.UseCases.Company;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AppointmentController(AppointmentService appointmentService, IHttpContextAccessor contextAccessor)
        {
            _appointmentService = appointmentService;
            _contextAccessor = contextAccessor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task AddAppointment([FromBody] AppointmentRequestModel appointment)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _appointmentService.AddAsync(appointment, userId);

        }
        
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CompanyAppointments")]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task<IEnumerable<AppointmentDto>> GettAllCompanyAppointments(int page)
        {
            Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            return await _appointmentService.GetAllCompanyAppointments(page, adminId);
        }



    }
}
