using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Entities.Reservation;

public class Reservation : Entity<Guid>
{
    public Appointment Appointment { get; set; }
    public bool IsFinished { get; set; } = false;
    public Customer Customer { get; set; }
    public List<ReservationEquipment> Equipments { get; set; }

    public Reservation()
    {

    }
    public Reservation(Appointment appointment, bool isFinished)
    {
        Appointment = appointment;
        IsFinished = isFinished;
    }
}
