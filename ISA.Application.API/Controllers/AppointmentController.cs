namespace ISA.Application.API.Controllers
{
    using ISA.Core.Domain.Dtos;
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
        public async Task AddAppointment([FromBody] AppointmentRequestModel appointment) => await _appointmentService.AddAsync(appointment);




    }
}
