using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class UserRepository : GenericRepository<User, Guid>, IUserRepository
{
    private readonly IsaDbContext _isaDbContext;
    public UserRepository(IsaDbContext isaDbContext) : base(isaDbContext)
    {
        _isaDbContext = isaDbContext;
    }

    public override async Task<User?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.Include(a => a.Address)
                           .Where(a => a.Id == Id)
                           .FirstOrDefaultAsync();
    }

}
