namespace ISA.Core.Domain.Contracts.Services;

public interface ILoyaltyProgramService
{
    Task CreateLoyaltyProgramAsync(Entities.LoyaltyProgram.LoyaltyProgram loyaltyProgram);
}
