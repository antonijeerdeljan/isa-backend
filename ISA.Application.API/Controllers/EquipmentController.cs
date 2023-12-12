namespace ISA.Application.API.Controllers
{
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.UseCases.Company;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly EquimpentService _equipmentService;
        private readonly IHttpContextAccessor _contextAccessor;
        public EquipmentController(EquimpentService equimpentService, IHttpContextAccessor contextAccessor)
        {
            _equipmentService = equimpentService;
            _contextAccessor = contextAccessor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Create")]
        //[Authorize(Policy = "Corpadmin")]
        public async Task AddAppointment([FromBody] Equipment equipment) => await _equipmentService.AddAsync(equipment);
    }
}
