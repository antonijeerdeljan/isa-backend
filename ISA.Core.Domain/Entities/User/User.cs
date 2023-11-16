namespace ISA.Core.Domain.Entities.User;

public class User : Entity<Guid>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}
