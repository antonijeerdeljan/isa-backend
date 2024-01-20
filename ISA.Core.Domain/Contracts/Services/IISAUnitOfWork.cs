namespace ISA.Core.Domain.Contracts.Services;

public interface IISAUnitOfWork
{
    public Task StartTransactionAsync();
    public Task CommitTransactionAsync();
    public Task SaveChangesAsync();
    public Task SaveAndCommitChangesAsync();
    public Task RollBackAsync();
}
