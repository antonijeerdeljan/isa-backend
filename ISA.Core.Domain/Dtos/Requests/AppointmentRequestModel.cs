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
            StartingDateTime = new DateTime(startingDateTime.Year, startingDateTime.Month, startingDateTime.Hour, startingDateTime.Hour, startingDateTime.Minute, 0);
            EndingDateTime = new DateTime(endingDateTime.Year, endingDateTime.Month, endingDateTime.Hour, endingDateTime.Hour, endingDateTime.Minute, 0);
        }
    }
}
