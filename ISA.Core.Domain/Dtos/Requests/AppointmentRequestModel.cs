namespace ISA.Core.Domain.Dtos
{
    public class AppointmentRequestModel
    {
        public Guid AdminId { get; set; }

        public DateTime StartingDateTime { get; set; }

        public DateTime EndingDateTime { get; set; }

        public AppointmentRequestModel(DateTime startingDateTime, DateTime endingDateTime, Guid adminId)
        {
            AdminId = adminId;
            StartingDateTime = startingDateTime;
            EndingDateTime = endingDateTime;
        }
    }
}
