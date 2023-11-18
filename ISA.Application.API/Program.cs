using ISA.Application.API.Startup;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Contracts.Repositories;
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ISADB");
builder.Services.AddDbContext<IsaDbContext>(options =>
    options.UseNpgsql(connectionString,
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("ISA.Application.API"))); // Specify the assembly containing migrations


builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ISA.Core.Infrastructure.Identity")));


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IIdentityServices, IdentityServices>();
builder.Services.AddTransient<IISAUnitOfWork, ISAUnitOfWork>();
builder.Services.AddTransient<ITokenGenerator, JwtGenerator>();


builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<UserManager<ApplicationUser>>();

//builder.Services.AddTransient<IdentityRole>
/*builder.Services.AddTransient<RoleManager<IdentityRole>>();*/



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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
