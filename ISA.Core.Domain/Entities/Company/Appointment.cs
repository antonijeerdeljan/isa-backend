namespace ISA.Core.Domain.Entities.Company;

using ISA.Core.Domain.Entities.User;

public class Appointment : Entity<Guid>
{
    public CompanyAdmin CompanyAdmin { get; set; }
    public DateTime DateTime { get; set; }
    public int Duration { get; set; }
    public Company Company {  get; set; }
    public Appointment() {}

    public Appointment(Company company, CompanyAdmin companyAdmin, DateTime dateTime, int duration)
    {
        Id = Guid.NewGuid();
        CompanyAdmin = companyAdmin;
        DateTime = dateTime;
        Duration = duration;
        Company = company;
    }
}
