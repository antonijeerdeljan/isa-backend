using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;

public class GenericRepository<TEntity, TKey> where TEntity : class
{
    protected readonly IsaDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(IsaDbContext isaDbContext)
    {
        _dbContext = isaDbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TKey Id)
    {
        return await _dbSet.FindAsync(Id);
    }

    public async Task AddAsync(TEntity? entity)
    {
        await _dbSet.AddAsync(entity);
        //await _dbContext.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        try
        {
        await _dbContext.SaveChangesAsync();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Update(TEntity entity)
    {
        try
        {
        _dbContext.Update(entity);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }


}
