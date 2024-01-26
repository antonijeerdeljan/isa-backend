using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ISA.Core.Domain.Entities.Reservation;public enum ReservationState
{
    Finished = 0,
    Pending = 1,
    Overdue = 2,
    Canceled = 3,

}
public class Reservation
{
    [Key]
    public Guid AppointmentId {  get; set; }
    
    [ForeignKey(nameof(AppointmentId))]
    public Appointment Appointment { get; set; }
    public Customer Customer { get; set; }
    public List<ReservationEquipment> Equipments { get; set; }
    public ReservationState State { get; set; } = ReservationState.Pending;

    public Reservation()
    {

    }
    public Reservation(Appointment appointment, ReservationState state)
    {
        Appointment = appointment;
        State = state;
    }

    public Reservation(Appointment appointment, Customer customer, List<ReservationEquipment> equipments)
    {
        Appointment = appointment;
        Customer = customer;
        Equipments = equipments;
    }

    public void SetAsOverdue()
    {
        State = ReservationState.Overdue;
    }

    public void SetAsCanceled()
    {
        State = ReservationState.Canceled;
    }
}
