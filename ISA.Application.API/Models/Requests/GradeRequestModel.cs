namespace ISA.Application.API.Models.Requests
{
    public class GradeRequestModel
    {
        public Guid CompanyId { get; set; }
        public int Rate { get; set; }
        public string Reason { get; set; }
        public string Text { get; set; }
    }
}
