using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts.Services;

namespace ISA.Core.Domain.UseCases.LoyaltyProgram;

public class LoyaltyProgramService : ILoyaltyProgramService
{
	private readonly ILoyaltyProgramRepository _loyaltyProgramRepository;
	public LoyaltyProgramService(ILoyaltyProgramRepository loyaltyProgramRepository)
	{
        _loyaltyProgramRepository = loyaltyProgramRepository;
	}

	public async Task CreateLoyaltyProgramAsync(Entities.LoyaltyProgram.LoyaltyProgram loyaltyProgram)
	{
		loyaltyProgram.Id = Guid.NewGuid();
		await _loyaltyProgramRepository.AddAsync(loyaltyProgram);
	}
}
