using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.LoyaltyProgram;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class LoyaltyProgramRepository : GenericRepository<LoyaltyProgram, Guid>, ILoyaltyProgramRepository
{
    public LoyaltyProgramRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {

    }
}
