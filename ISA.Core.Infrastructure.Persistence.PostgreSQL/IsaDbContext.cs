using ISA.Core.Domain.Entities.Company;
using ISA.Core.Domain.Entities.Delivery;
using ISA.Core.Domain.Entities.LoyaltyProgram;
using ISA.Core.Domain.Entities.Reservation;
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
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<LoyaltyProgram> LoyaltyPrograms { get; set; }
    public DbSet<CompanyAdmin> CompanyAdmins { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<ReservationEquipment>()
        .HasKey(ae => new { ae.ReservationId, ae.EquipmentId }); 

        modelBuilder.Entity<ReservationEquipment>()
            .HasOne(ae => ae.Reservation) 
            .WithMany(a => a.Equipments) 
            .HasForeignKey(ae => ae.ReservationId);

        modelBuilder.Entity<ReservationEquipment>()
            .HasOne(ae => ae.Equipment)
            .WithMany() 
            .HasForeignKey(ae => ae.EquipmentId);

        modelBuilder.Entity<ContractEquipment>()
        .HasKey(ae => new { ae.ContractId, ae.EquipmentId });

        modelBuilder.Entity<ContractEquipment>()
            .HasOne(ae => ae.Contract)
            .WithMany(a => a.Equipments)
            .HasForeignKey(ae => ae.ContractId);

        modelBuilder.Entity<ContractEquipment>()
            .HasOne(ae => ae.Equipment)
            .WithMany()
            .HasForeignKey(ae => ae.EquipmentId);

    }

}
