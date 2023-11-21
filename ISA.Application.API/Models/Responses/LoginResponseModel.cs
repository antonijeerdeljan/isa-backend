using ISA.Core.Domain.Entities.Token;

namespace ISA.Application.API.Models.Responses
{
    public class LoginResponseModel
    {
        public AuthenticationTokens AuthToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

    }
}
