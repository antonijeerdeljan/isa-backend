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

        modelBuilder.Entity<AppointmentEquipment>()
        .HasKey(ae => new { ae.AppointmentId, ae.EquipmentId }); //kompozitni kljuc

        modelBuilder.Entity<AppointmentEquipment>()
            .HasOne(ae => ae.Appointment) //appointment equipment ima jedan appoitment
            .WithMany(a => a.Equipments) // jedan appointment moze imati vise appointmentequpmenta?
            .HasForeignKey(ae => ae.AppointmentId);

        modelBuilder.Entity<AppointmentEquipment>()
            .HasOne(ae => ae.Equipment)
            .WithMany() 
            .HasForeignKey(ae => ae.EquipmentId);

    }

}
