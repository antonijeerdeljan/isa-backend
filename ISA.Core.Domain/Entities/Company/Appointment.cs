namespace ISA.Core.Domain.Entities.Company;

using ISA.Core.Domain.Entities.User;

public class Appointment : Entity<Guid>
{
    public CompanyAdmin CompanyAdmin { get; set; }
    public DateTime StartingDateTime { get; set; }
    public DateTime EndingDateTime { get; set; }
    public Company Company { get; set; }
    public bool AlreadyTaken { get; set; } = false;
    public Appointment() { }


    public Appointment(Company company, CompanyAdmin companyAdmin, DateTime startingDateTime, DateTime endingDateTime)
    {
        Id = Guid.NewGuid();
        CompanyAdmin = companyAdmin;
        StartingDateTime = startingDateTime;
        EndingDateTime = endingDateTime;
        Company = company;
    }

    public Appointment(Appointment appointment)
    {
        Id = Guid.NewGuid();
        CompanyAdmin = appointment.CompanyAdmin;
        StartingDateTime = appointment.StartingDateTime;
        EndingDateTime = appointment.EndingDateTime;
        Company = appointment.Company;
    }

    public void SetAsTaken()
    {
        AlreadyTaken = true;
    }

    public void SetAsAvailable()
    {
        AlreadyTaken = false;
    }

}
