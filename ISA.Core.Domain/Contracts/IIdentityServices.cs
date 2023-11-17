namespace ISA.Core.Domain.Contracts;

public interface IIdentityServices
{
    public Task RegisterAsync(Guid id, string email, string password);

}
