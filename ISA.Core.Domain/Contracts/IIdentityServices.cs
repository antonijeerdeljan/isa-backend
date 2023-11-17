namespace ISA.Core.Domain.Contracts;

public interface IIdentityServices
{
    public Task RegisterUserAsync(Guid id, string email, string password, string userRole);

}
