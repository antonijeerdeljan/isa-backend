using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.LoyaltyProgram;
using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL;

public class IsaDbContext : DbContext
{
	public IsaDbContext(DbContextOptions<IsaDbContext> options) : base(options)
	{
		
	}

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<LoyaltyProgram> LoyaltyPrograms { get; set; }
    public DbSet<CompanyAdmin> CompanyAdmins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
    
        modelBuilder.Entity<Appointment>()
            .HasOne(c => c.Id)
            .WithMany()

    }

}
