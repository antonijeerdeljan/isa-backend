using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.Identity;
using ISA.Core.Infrastructure.Identity.Entities;
using ISA.Core.Infrastructure.Identity.Services;
using ISA.Core.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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


builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IIdentityServices, IdentityServices>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();


builder.Services.AddIdentity<ApplicationUser, ISA.Core.Infrastructure.Identity.Entities.IdentityRole>(o =>
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
