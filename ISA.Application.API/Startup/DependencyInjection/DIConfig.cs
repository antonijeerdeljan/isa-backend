using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.HttpClients;
using ISA.Core.Infrastructure.Identity.Entities;
using ISA.Core.Infrastructure.Identity.Services;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;
using ISA.Core.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;

namespace ISA.Application.API.Startup.DI;

public static class DIConfig
{
    public static IServiceCollection AddDICconfig(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICompanyRepository, CompanyRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IIdentityServices, IdentityServices>();
        services.AddTransient<IISAUnitOfWork, ISAUnitOfWork>();
        services.AddTransient<ITokenGenerator, JwtGenerator>();
        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<UserService>();
        services.AddTransient<CompanyService>();
        services.AddTransient<UserManager<ApplicationUser>>();
        services.AddScoped<RoleManager<ApplicationRole>>();
        services.AddTransient<SignInManager<ApplicationUser>>();

        return services;
    }
}
