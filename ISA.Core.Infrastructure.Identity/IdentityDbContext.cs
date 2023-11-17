using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityRole = ISA.Core.Infrastructure.Identity.Entities.IdentityRole;

namespace ISA.Core.Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole, Guid>
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}
