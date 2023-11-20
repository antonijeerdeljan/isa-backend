using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ISA.Application.API.Startup;

public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services)
    {
        ConfigureAuthentication(services);
        ConfigureAuthorizationPolicies(services);
        return services;
    }

    private static void ConfigureAuthentication(IServiceCollection services)
    {
        var key = Environment.GetEnvironmentVariable("secret") ?? "secretqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq";
        var issuer = Environment.GetEnvironmentVariable("validIssuer") ?? "localhost";
        var audience = Environment.GetEnvironmentVariable("validAudience") ?? "localhost";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("AuthenticationTokens-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };

                options.Events.OnMessageReceived = context =>
                {
                    context.Token = context.HttpContext.Request.Headers["X-JWT-Assertion"];
                    return Task.CompletedTask;
                };

            });
    }

    private static void ConfigureAuthorizationPolicies(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("customerPolicy", policy => policy.RequireRole("Customer"));
            options.AddPolicy("corpAdminPolicy", policy => policy.RequireRole("Corpadmin"));
            options.AddPolicy("superAdminPolicy", policy => policy.RequireRole("Sysadmin"));

            options.AddPolicy("allowAllPolicy", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.IsInRole("Customer") ||
                    context.User.IsInRole("Corpadmin") ||
                    context.User.IsInRole("Sysadmin")
                );
            });

        });
    }

}
