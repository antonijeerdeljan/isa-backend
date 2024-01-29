using ISA.Core.Domain.UseCases.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace ISA.Application.API.Startup
{
    public static class AuthConfig
    {
        // Configures both authentication and authorization services
        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            ConfigureAuthentication(services);
            ConfigureAuthorizationPolicies(services);
            return services;
        }

        // Sets up JWT Authentication
        private static void ConfigureAuthentication(IServiceCollection services)
        {
            // Retrieve key, issuer, and audience from environment variables
            var key = Environment.GetEnvironmentVariable("secret") ?? "default_secret_keydefault_secret_key";
            var issuer = Environment.GetEnvironmentVariable("validIssuer") ?? "localhost";
            var audience = Environment.GetEnvironmentVariable("validAudience") ?? "localhost";

            services.AddAuthentication(options =>
            {
                // Setting the default authentication and challenge scheme to JWT Bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Token validation parameters setup
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // JWT bearer events configuration
                options.Events = new JwtBearerEvents
                {
                    // On receiving a message, check for access_token in the query
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },

                    // On authentication failure, handle token expiration and refresh
                    OnAuthenticationFailed = async context =>
                    {
                        HandleAuthenticationFailure(context, services);
                    }
                };
            });
        }


        // Method to handle authentication failure events
        private static async Task HandleAuthenticationFailure(AuthenticationFailedContext context, IServiceCollection services)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var serviceProvider = services.BuildServiceProvider();
            var userService = serviceProvider.GetRequiredService<UserService>();

            // Extract user details from the token
            if (!string.IsNullOrEmpty(token))
            {
                var (userId, userRole) = ExtractUserDetailsFromToken(token);
                if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(userRole))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            // Handle expired security token
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                await HandleExpiredSecurityToken(context, userService);
            }
        }

        // Extracts user details from a given JWT token
        private static (string userId, string userRole) ExtractUserDetailsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaims = tokenHandler.ReadJwtToken(token)?.Payload;

            if (tokenClaims != null)
            {
                var userId = tokenClaims.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                var userRole = tokenClaims.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                return (userId, userRole);
            }

            return (string.Empty, string.Empty);
        }

        // Handles an expired security token scenario
        private static async Task HandleExpiredSecurityToken(AuthenticationFailedContext context, UserService userService)
        {
            var refreshToken = context.HttpContext.Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var userId = ExtractUserDetailsFromToken(context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")).userId;
                bool isRefreshTokenValid = await userService.IsRefreshTokenValid(userId, refreshToken);

                if (isRefreshTokenValid)
                {
                    var newJwt = userService.GenerateNewJWT(userId, "role_placeholder"); // Replace 'role_placeholder' with actual role
                    context.Response.Headers.Add("X-New-Token", newJwt.AccessToken);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }
        }

        // Sets up Authorization Policies
        private static void ConfigureAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("customerPolicy", policy => policy.RequireRole("Customer"));
                options.AddPolicy("corpAdminPolicy", policy => policy.RequireRole("Corpadmin"));
                options.AddPolicy("superAdminPolicy", policy => policy.RequireRole("Sysadmin"));

                // Policy that allows all specified roles
                options.AddPolicy("allowAllPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Customer") ||
                        context.User.IsInRole("Corpadmin") ||
                        context.User.IsInRole("Sysadmin")
                    );
                });

                // Policy exclusive to admins
                options.AddPolicy("AdminsPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Corpadmin") ||
                        context.User.IsInRole("Sysadmin")
                    );
                });
            });
        }
    }
}
