using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Infrastructure.Identity;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL;

public class ISAUnitOfWork : IISAUnitOfWork
{
    private readonly IsaDbContext _isaDbContext;
    private readonly IdentityDbContext _identityDbContext;

    public ISAUnitOfWork(IsaDbContext isaDbContext, IdentityDbContext identityDbContext)
    {
        _isaDbContext = isaDbContext;
        _identityDbContext = identityDbContext;
    }

    public async Task StartTransactionAsync()
    {
        await _isaDbContext.Database.BeginTransactionAsync();
        await _identityDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _isaDbContext.Database.CommitTransactionAsync();
        await _identityDbContext.Database.CommitTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _isaDbContext.SaveChangesAsync();
        await _identityDbContext.SaveChangesAsync();
    }

    public async Task SaveAndCommitChangesAsync()
    {
        try {
            await _isaDbContext.SaveChangesAsync();
            await _identityDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        await _isaDbContext.Database.CommitTransactionAsync();
        await _identityDbContext.Database.CommitTransactionAsync();
    }

    public async Task RollBackAsync()
    {
        await _identityDbContext.Database.RollbackTransactionAsync();
        await _isaDbContext.Database.RollbackTransactionAsync();
    }
}
