namespace ISA.Core.Domain.Entities.Token;

public class RefreshToken
{
    public Guid Id { get; set; }
    public DateTime ExpirationDate { get; set; }
}

