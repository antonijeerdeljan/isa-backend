namespace ISA.Core.Domain.Dtos
{
    public class AppointmentRequestModel
    {
        public Guid CompanyId { get; set; }
        public Guid AdminId { get; set; }

        public DateTime StartingDateTime { get; set; }

        public DateTime EndingDateTime { get; set; }
    }
}
