using ISA.Core.Domain.Entities;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface ICustomerRepository
{
    public Task AddAsync(Customer userToAdd);
    public Task<Customer?> GetByIdAsync(Guid id);
    public Task SaveAsync();

    public void Update(Customer customer);
}
