namespace ISA.Application.API.Controllers
{
    using ISA.Application.API.Models.Requests;
    using ISA.Core.Domain.Contracts.Services;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.Dtos.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Entities.User;
    using ISA.Core.Domain.UseCases.Company;
    using ISA.Core.Domain.UseCases.Reservation;
    using ISA.Core.Domain.UseCases.User;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReservationController(ReservationService reservationService, IHttpContextAccessor contextAccessor)
        {
            _reservationService = reservationService;
            _contextAccessor = contextAccessor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("add")]
        [Authorize(Policy = "customerPolicy")]
        public async Task AddReservation([FromBody] ReservationRequest reservation)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _reservationService.AddAsync(userId, reservation.AppointmentId, reservation.Equipments);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("cancel")]
        [Authorize(Policy = "customerPolicy")]
        public async Task CancelReservation([FromBody] Guid reservationId)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _reservationService.CancelReservation(userId, reservationId);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CompanyReservations")]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task<IEnumerable<ReservationDto>> GetAllCompanyReservations()
        {
            Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            return await _reservationService.GetAllCompanyReservations(adminId);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ReservationPickedUp/{reservationId}")]
        [Authorize(Policy = "corpAdminPolicy")]
        public async Task ReservationPickedUp([FromRoute] Guid reservationId)
        {
            Guid adminId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _reservationService.ReservationPickedUp(adminId, reservationId);
        }

    }
}
    

