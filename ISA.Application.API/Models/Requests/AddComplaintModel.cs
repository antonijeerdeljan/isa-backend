namespace ISA.Application.API.Models.Requests;

public class AddComplaintModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid ComplaintObjectId { get; set; }
}
