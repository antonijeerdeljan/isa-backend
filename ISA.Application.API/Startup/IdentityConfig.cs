using ISA.Core.Infrastructure.Identity;
using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ISA.Application.API.Startup;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
        {
            o.Password.RequiredLength = 8;
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = true;
            o.User.RequireUniqueEmail = true;
            o.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
