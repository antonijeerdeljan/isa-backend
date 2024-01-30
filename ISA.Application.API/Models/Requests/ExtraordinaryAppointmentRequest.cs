namespace ISA.Application.API.Models.Requests
{
    public class ExtraordinaryAppointmentRequest
    {
        public DateTime StartingDateTime { get; set; }

        public DateTime EndingDateTime { get; set; }

        public ExtraordinaryAppointmentRequest(DateTime startingDateTime, DateTime endingDateTime)
        {
            StartingDateTime = startingDateTime;
            EndingDateTime = endingDateTime;
        }
    }
}
