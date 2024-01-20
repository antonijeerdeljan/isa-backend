namespace ISA.Core.Domain.Entities.User;

public class Customer : Entity<Guid>
{
    public string Profession { get; set; }
    public string CompanyInfo { get; set; }
    public Entities.LoyaltyProgram.LoyaltyProgram? LoyaltyProgram { get; set; }
    public int? PenaltyPoints { get; set; } = 0;
    public int? Points { get; set; } = 0;
    public User User { get; set; }

    public Customer()
    {

    }

    public Customer(Guid id,string profession, string companyInfo, User user)
    {
        Id = Id;
        Profession = profession;
        CompanyInfo = companyInfo;
        User = user;
    }
}
