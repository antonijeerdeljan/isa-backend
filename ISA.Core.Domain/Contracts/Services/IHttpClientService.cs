namespace ISA.Core.Domain.Contracts.Services;

public interface IHttpClientService
{
    public Task SendEmail(string email, string message);
}
