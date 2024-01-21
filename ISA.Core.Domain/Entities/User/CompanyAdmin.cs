namespace ISA.Core.Domain.Entities.User;

public class CompanyAdmin : Entity<Guid>
{
    public User User { get; set; }
    public Company.Company Company { get; set; }

    public CompanyAdmin()
    {
    }

    public CompanyAdmin(User user, Company.Company company)
    {
        User = user;
        Company = company;
    }
}
