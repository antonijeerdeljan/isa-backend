namespace ISA.Application.API.Models.Requests;

public record CompanyUpdateRequest(string Name, string City, string Country, string Street, int Number, string Description);

