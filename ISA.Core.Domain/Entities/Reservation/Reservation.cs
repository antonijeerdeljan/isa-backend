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
    public Guid AppointmentId {  get; set; }
    
    [ForeignKey(nameof(AppointmentId))]
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

    public Reservation(Appointment appointment, Customer customer, List<ReservationEquipment> equipments)
    {
        Appointment = appointment;
        Customer = customer;
        Equipments = equipments;
    }
}
