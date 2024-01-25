using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Core.Domain.Entities.Reservation;

public class Reservation
{
    //napriviti da id bude appoinemnt id
    //resiti datume za dodavanje appoinemtenta
    //resitii datume

    [Key]
    [ForeignKey("Appointment")] 
    public Guid AppointmentID { get; set; }
    public virtual Appointment Appointment { get; set; }
    public bool IsFinished { get; set; } = false;
    public virtual Customer Customer { get; set; }
    public virtual List<ReservationEquipment> Equipments { get; set; }

    public Reservation()
    {

    }
    public Reservation(Appointment appointment, bool isFinished)
    {
        Appointment = appointment;
        IsFinished = isFinished;
    }

    public Reservation(Appointment appointment, Customer customer, List<ReservationEquipment> equipments)
    {
        Appointment = appointment;
        Customer = customer;
        Equipments = equipments;
    }
}
