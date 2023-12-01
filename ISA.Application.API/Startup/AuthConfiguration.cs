using ISA.Core.Domain.UseCases.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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
                    OnAuthenticationFailed = async context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var serviceProvider = services.BuildServiceProvider();
                        var userService = serviceProvider.GetRequiredService<UserService>();

                        var userId = string.Empty;
                        var userRole = string.Empty;

                        if (!string.IsNullOrEmpty(token))
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var tokenClaims = tokenHandler.ReadJwtToken(token)?.Payload;

                            if (tokenClaims != null)
                            {
                                userId = tokenClaims.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                                userRole = tokenClaims.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                                if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(userRole))
                                    return;
                            }
                        }
                        else
                            return;


                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var refreshToken = context.HttpContext.Request.Cookies["RefreshToken"];
                            if (!string.IsNullOrEmpty(refreshToken))
                            {
                                // Check and validate the refresh token
                                bool isRefreshTokenValid = await userService.IsRefreshTokenValid(userId,refreshToken); 

                                if (isRefreshTokenValid)
                                {
                                    var newJwt = userService.GenerateNewJWT(userId, userRole);
                                    context.Response.Headers.Add("X-New-Token", newJwt.AccessToken);
                                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                    return;
                                }
                            }
                        }
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
