namespace ISA.Core.Domain.Entities.User;

public class CompanyAdmin : Entity<Guid>
{
    public Entities.Company.Company Company { get; set; }

    public User User;
	public CompanyAdmin() : base()
    {

    }

    public CompanyAdmin(User user, Company.Company company)
    {
        User = user;
        Company = company;
    }
}
