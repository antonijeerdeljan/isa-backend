namespace ISA.Core.Domain.Contracts;

public interface IHttpClientService
{
    public Task SendEmail(string email, string message);
}
