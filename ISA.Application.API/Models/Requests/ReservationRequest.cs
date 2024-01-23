namespace ISA.Application.API.Models.Requests
{
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Entities.User;

    public class ReservationRequest
    {
        public Guid AppointmentId { get; set; }
        public List<ReservationEquipmentRequest> Equipments { get; set; }
    }
}
