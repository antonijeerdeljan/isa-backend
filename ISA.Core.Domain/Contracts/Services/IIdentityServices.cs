using FluentResults;
using ISA.Core.Domain.Entities.Token;

namespace ISA.Core.Domain.Contracts.Services;

public interface IIdentityServices
{
    public Task RegisterUserAsync(Guid id, string email, string password, string userRole);
    public Task<LoginCookie> LoginAsync(string email, string password);
    public Task<bool> VerifyRefreshToken(string id, string token);
    public Task VerifyEmail(string email, string token);
    public AuthenticationTokens GenerateNewJWT(string userId, string userRole);
    public Task ChangePasswordAsync(string email, string passwordToken, string newPassword);

}
