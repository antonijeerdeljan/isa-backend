namespace ISA.Application.API.Models.Requests;

public record ComplaintAnswerRequest(Guid complaintId, string answer);
