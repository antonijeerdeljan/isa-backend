namespace ISA.Core.Domain.Dtos
{
    using System;

    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
        public AppointmentDto() { }
    }
}
