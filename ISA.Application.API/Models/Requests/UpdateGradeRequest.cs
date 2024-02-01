namespace ISA.Application.API.Models.Requests
{
    public class UpdateGradeRequest
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Reason { get; set; }
        public string Text { get; set; }
    }
}
