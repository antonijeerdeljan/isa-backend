namespace ISA.Core.Domain.Dtos
{
    public class AppointmentRequestModel
    {
        public Guid CompanyId { get; set; }
        public Guid AdminId { get; set; } //Koji ce biti na preuzimanju
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
    }
}
