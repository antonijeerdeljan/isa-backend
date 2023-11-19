namespace ISA.Core.Domain.Entities.User;

public class Customer : Entity<Guid>
{
    public string Profession { get; set; }
    public string CompanyInfo { get; set; }
    public User User { get; set; }

    public Customer()
    {

    }

    public Customer(string profession, string companyInfo, User user)
    {
        Profession = profession;
        CompanyInfo = companyInfo;
        User = user;
    }
}
