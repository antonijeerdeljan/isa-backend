namespace ISA.Core.Infrastructure.HttpClients.Entities;

public class EmailMessagePayload
{
    public string Email { get; set; }
    public string Message { get; set; }

    public string Body { get; set; }

    public string Name {  get; set; }
    public string Id { get; set; }
    public string Time { get; set; }
    public string CompanyName {  get; set; }
}
