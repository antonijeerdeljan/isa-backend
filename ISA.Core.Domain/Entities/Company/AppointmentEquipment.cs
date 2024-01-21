namespace ISA.Core.Domain.Entities.Company;

public class AppointmentEquipment
{
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
}
