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
            StartingDateTime = new DateTime(startingDateTime.Year, startingDateTime.Month, startingDateTime.Day, startingDateTime.Hour, startingDateTime.Minute, 0).ToUniversalTime();
            EndingDateTime = new DateTime(endingDateTime.Year, endingDateTime.Month, endingDateTime.Day, endingDateTime.Hour, endingDateTime.Minute, 0).ToUniversalTime();
        }
    }
}
