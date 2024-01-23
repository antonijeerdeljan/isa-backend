namespace ISA.Application.API.Controllers
{
    using ISA.Application.API.Models.Requests;
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
        private readonly EquipmentService _equipmentService;
        private readonly IHttpContextAccessor _contextAccessor;
        public EquipmentController(EquipmentService equimpentService, IHttpContextAccessor contextAccessor)
        {
            _equipmentService = equimpentService;
            _contextAccessor = contextAccessor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task AddEquipment([FromBody] AddEquipmentRequest equipment)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _equipmentService.AddAsync(equipment.Name, equipment.Quantity, userId);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task UpdateEquipment([FromBody] UpdateEquipmentRequest equipment)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _equipmentService.UpdateAsync(equipment.Id, equipment.Name, equipment.Quantity, userId);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("/{id}")]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task RemoveEquipment([FromRoute] Guid id) => await _equipmentService.RemoveAsync(id);

    }
}
