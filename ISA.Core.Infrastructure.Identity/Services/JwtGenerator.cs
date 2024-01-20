using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ISA.Core.Infrastructure.Identity.Services;

public class JwtGenerator : ITokenGenerator
{
    private readonly string key = Environment.GetEnvironmentVariable("secret") ?? "secretqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq";
    private readonly string issuer = Environment.GetEnvironmentVariable("validIssuer") ?? "localhost";
    private readonly string audience = Environment.GetEnvironmentVariable("validAudience") ?? "localhost";

    public AuthenticationTokens GenerateAccessToken(string userId, string userRole)
    {
        var authenticationResponse = new AuthenticationTokens();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id", userId),
            new(ClaimTypes.Role, userRole)
        };

        var jwt = CreateToken(claims, 100);
        authenticationResponse.Id = userId;
        authenticationResponse.AccessToken = jwt;

        return authenticationResponse;
    }

    private string CreateToken(IEnumerable<Claim> claims, double expirationTimeInMinutes)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
