using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL;

public class IsaDbContext : DbContext
{
	public IsaDbContext(DbContextOptions<IsaDbContext> options) : base(options)
	{
		
	}

    public DbSet<User> Users { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("YourConnectionString");
        }
    }*/
}
