namespace ISA.Application.API.Models.Requests
{
    using ISA.Core.Domain.Entities.Company;
    using ISA.Core.Domain.Entities.Reservation;

    public class ReservationEquipmentRequest
    {
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
    }
}
