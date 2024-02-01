namespace ISA.Application.API.Models.Requests
{
    public class ExtraordinaryAppointmentRequest
    {
        public DateTime StartingDateTime { get; set; }

        public DateTime EndingDateTime { get; set; }

        public ExtraordinaryAppointmentRequest(DateTime startingDateTime, DateTime endingDateTime)
        {
            StartingDateTime = new DateTime(startingDateTime.Year, startingDateTime.Month, startingDateTime.Day, startingDateTime.Hour, startingDateTime.Minute, 0);
            EndingDateTime = new DateTime(endingDateTime.Year, endingDateTime.Month, endingDateTime.Day, endingDateTime.Hour, endingDateTime.Minute, 0);
        }
    }
}
