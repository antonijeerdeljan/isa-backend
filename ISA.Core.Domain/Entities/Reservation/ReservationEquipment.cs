using ISA.Core.Domain.Entities.Company;

namespace ISA.Core.Domain.Entities.Reservation;

public class ReservationEquipment
{
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; }
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; }

    public int Quantity { get; set; }

    public ReservationEquipment()
    {
    }

    public ReservationEquipment(Guid reservationId, Guid equipmentId, int quantity)
    {
        ReservationId = reservationId;
        EquipmentId = equipmentId;
        Quantity = quantity;
    }
}
