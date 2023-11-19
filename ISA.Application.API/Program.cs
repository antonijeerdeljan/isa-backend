using ISA.Application.API.Startup;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using ISA.Core.Infrastructure.Identity.Entities;
using ISA.Core.Infrastructure.Identity.Services;
using ISA.Core.Infrastructure.Persistence.PostgreSQL;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureAuth();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();


var connectionString = builder.Configuration.GetConnectionString("ISADB");
builder.Services.AddDbContext<IsaDbContext>(options =>
    options.UseNpgsql(connectionString,
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("ISA.Application.API"))); // Specify the assembly containing migrations

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ISA.Core.Infrastructure.Identity")));


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<IIdentityServices, IdentityServices>();
builder.Services.AddTransient<IISAUnitOfWork, ISAUnitOfWork>();
builder.Services.AddTransient<ITokenGenerator, JwtGenerator>();


builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<UserManager<ApplicationUser>>();



builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
{
    o.Password.RequiredLength = 8;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = true;
    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddTransient<SignInManager<ApplicationUser>>();
builder.Services.AddAuthentication();

    /*.AddJwtBearer(configureOptions =>
    {
        configureOptions.Events.OnMessageReceived = context =>
        {
            context.Token = context.HttpContext.Request.Headers["X-JWT-Assertion"];
            return Task.CompletedTask;
        };
    });*/


/*builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});*/


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
