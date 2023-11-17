using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL;

public class IsaDbContext : DbContext
{
	public IsaDbContext(DbContextOptions<IsaDbContext> options) : base(options)
	{
		
	}

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne()
            .HasForeignKey<Address>(u => u.Id);
    }

}
