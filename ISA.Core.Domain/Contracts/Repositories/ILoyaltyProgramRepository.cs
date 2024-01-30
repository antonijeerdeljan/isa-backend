using ISA.Core.Domain.Entities.LoyaltyProgram;

namespace ISA.Core.Domain.Contracts.Repositories;

public interface ILoyaltyProgramRepository
{
    Task AddAsync(LoyaltyProgram loyaltyProgram);
    Task<LoyaltyProgram?> GetByIdAsync(Guid id);
    void UpdateAndSaveChanges(LoyaltyProgram loyaltyProgram);
    void Update(LoyaltyProgram loyaltyProgram);
}
