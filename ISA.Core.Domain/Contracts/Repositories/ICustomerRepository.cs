using ISA.Core.Domain.Entities;
using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface ICustomerRepository
{
    public Task AddAsync(Customer userToAdd);
    public Task<Customer?> GetByIdAsync(Guid id);
    public Task SaveAsync();
    Task RemovePenaltyPoints();
    public void Update(Customer customer);

    Task<IEnumerable<Customer>> GetAllCompanyCustomers(List<Guid> usersId);
}
