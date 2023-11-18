﻿using ISA.Core.Domain.Contracts;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL;

public class ISAUnitOfWork : IISAUnitOfWork
{
    private readonly IsaDbContext _isaDbContext;

	public ISAUnitOfWork(IsaDbContext isaDbContext)
	{
		_isaDbContext = isaDbContext;
	}

    public async Task StartTransactionAsync() => await _isaDbContext.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync() => await _isaDbContext.Database.CommitTransactionAsync();

    public async Task SaveChangesAsync() => await _isaDbContext.SaveChangesAsync();

    public async Task SaveAndCommitChangesAsync()
    {
        await _isaDbContext.SaveChangesAsync();
        await _isaDbContext.Database.CommitTransactionAsync();
    }
}
