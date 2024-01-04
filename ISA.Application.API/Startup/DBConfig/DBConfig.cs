using ISA.Core.Infrastructure.Identity;
using ISA.Core.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace ISA.Application.API.Startup.DBConfiguration;

public static class DBConfig
{
    public static IServiceCollection AddDbConfig(this IServiceCollection services, string? connectionString)
    {

        if(connectionString is null)
            throw new ArgumentNullException(nameof(connectionString));

        services.AddDbContext<IsaDbContext>(options =>
            options.UseNpgsql(connectionString,
            npgsqlOptions => npgsqlOptions.MigrationsAssembly("ISA.Application.API"))); // Specify the assembly containing migrations

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ISA.Core.Infrastructure.Identity")));

        return services;
    }
}
