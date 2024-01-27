namespace ISA.Core.Domain.Dtos
{
    using ISA.Core.Domain.Entities.Reservation;
    using System.Collections.Generic;

    public class ReservationDto
    {
        public AppointmentDto Appointment { get; set; }
        public ReservationState State { get; set; }
    }
}
