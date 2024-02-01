using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Entities.Complaint;


public enum ComplaintType
{
    Admin = 0,
    Compnay = 1

}
public class Complaint : Entity<Guid>
{
    public ComplaintType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid SubjectComplaintId { get; set; }
    public Customer Customer { get; set; }
    public bool Resloved { get; set; } = false;
    public string? Answer { get; set; }
    public CompanyAdmin? AnsweredBy { get; set; }


    public Complaint()
    {
        Id = Guid.NewGuid();
    }

    public Complaint(ComplaintType type, string title, Customer customer, string description, Guid subjectComplaintId)
    {
        Id = Guid.NewGuid();
        Type = type;
        Title = title;
        Description = description;
        Customer = customer;
        SubjectComplaintId = subjectComplaintId;
    }

    public void AnswerComplaint(string answer, CompanyAdmin admin)
    {
        Answer = answer;
        AnsweredBy = admin;
        Resloved = true;
    }
}
