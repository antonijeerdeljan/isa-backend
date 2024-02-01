namespace ISA.Application.API.Models.Requests
{
    public class ReservationRequest
    {
        public Guid AppointmentId { get; set; }
        public List<ReservationEquipmentRequest> Equipments { get; set; }
    }
}
