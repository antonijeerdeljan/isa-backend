using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface IUserRepository
{
    public Task AddAsync(User userToAdd);
    public Task<User?> GetByIdAsync(Guid id);
}
