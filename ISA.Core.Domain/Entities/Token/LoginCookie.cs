namespace ISA.Core.Domain.Entities.Token;

public class LoginCookie
{
    public AuthenticationTokens AuthToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
