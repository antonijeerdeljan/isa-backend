namespace ISA.Core.Domain.Dtos
{
    using System;

    public class AppointmentDto
    {
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
        public AppointmentDto() { }
    }
}
