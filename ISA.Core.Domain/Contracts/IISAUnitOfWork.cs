namespace ISA.Core.Domain.Contracts;

public interface IISAUnitOfWork
{
    public Task StartTransactionAsync();
    public Task CommitTransactionAsync();
    public Task SaveChangesAsync();
    public Task SaveAndCommitChangesAsync();
}
