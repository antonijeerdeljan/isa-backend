namespace ISA.Core.Domain.Contracts.Repositories;

public interface IContractRepository
{
    public Task AddAsync(Entities.Delivery.Contract userToAdd);
    public Task<Entities.Delivery.Contract?> GetByIdAsync(Guid id);
    public Task SaveAsync();
    public void Update(Entities.Delivery.Contract customer);
    Task<List<Entities.Delivery.Contract?>> GetTodaysContract();
    Task DeleteContract(Guid id);
    Task<Entities.Delivery.Contract?> GetContractByCompanyAsync(Guid companyId);
    Task<bool> CompanyAlreadyHaveContract(Guid companyId);
}
