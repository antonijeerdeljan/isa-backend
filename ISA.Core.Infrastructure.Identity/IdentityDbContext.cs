using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public DbSet<ApplicationUser> SuperUser { get; set; }


    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);



        builder.Entity<ApplicationRole>().HasData(
        new ApplicationRole
        {
            Id = Guid.Parse("5310feb4-a1e1-4439-b511-fd2293f33af2"),
            Name = IdentityRoles.CUSTOMER,
            NormalizedName = IdentityRoles.CUSTOMER.ToUpper(),
        },
        new ApplicationRole
        {
            Id = Guid.Parse("5310feb4-a1e1-4439-b511-fd2293f33af0"),
            Name = IdentityRoles.CORPADMIN,
            NormalizedName = IdentityRoles.CORPADMIN.ToUpper(),
        },
        new ApplicationRole
        {
            Id = Guid.Parse("5310feb4-a1e1-4439-b511-fd2293f33af1"),
            Name = IdentityRoles.SYSADMIN,
            NormalizedName = IdentityRoles.SYSADMIN.ToUpper(),
        });


    }
}
