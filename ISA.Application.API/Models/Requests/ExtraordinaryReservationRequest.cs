namespace ISA.Application.API.Models.Requests
{
    using ISA.Core.Domain.Dtos;

    public class ExtraordinaryReservationRequest
    {
        public Guid CompanyId { get; set; }
        public ExtraordinaryAppointmentRequest Appointment { get; set; }
        public List<ReservationEquipmentRequest> Equipments { get; set; }
    }
}
