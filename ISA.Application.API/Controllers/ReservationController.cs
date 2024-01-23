namespace ISA.Application.API.Controllers
{
    using ISA.Application.API.Models.Requests;
    using ISA.Core.Domain.Dtos;
    using ISA.Core.Domain.UseCases.Company;
    using ISA.Core.Domain.UseCases.Reservation;
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
        [HttpPost]
        [Authorize(Policy = "customerPolicy")]
        public async Task AddReservation([FromBody] ReservationRequest reservation)
        {
            Guid userId = Guid.Parse(User.Claims.First(x => x.Type == "id").Value);
            await _reservationService.AddAsync(userId, reservation.AppointmentId, reservation.Equipments);
        }

    }
}
    

