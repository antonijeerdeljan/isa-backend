namespace ISA.Application.API.Models.Requests;

public class AddEqupmentRequest
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public Guid CompanyId { get; set; }
}
