namespace ISA.Core.Domain.Dtos.Complaint;

public record ComplaintDto(Guid Id, string Title, string Description, string? Answer);