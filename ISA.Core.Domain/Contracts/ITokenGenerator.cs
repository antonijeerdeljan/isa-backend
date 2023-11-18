using ISA.Core.Domain.Entities.Token;
using ISA.Core.Domain.Entities.User;
namespace ISA.Core.Domain.Contracts;

public interface ITokenGenerator
{
    public AuthenticationTokens GenerateAccessToken(string userId, string userRole);

}
