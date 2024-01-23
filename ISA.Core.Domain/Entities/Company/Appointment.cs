namespace ISA.Core.Domain.Entities.Company
{
    using ISA.Core.Domain.Entities.User;
    using System.ComponentModel;

    public class Appointment : Entity<Guid>
    {
        public CompanyAdmin CompanyAdmin { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
        public Company Company {  get; set; }
        public Customer? Customer { get; set; }
        public List<AppointmentEquipment> Equipments { get; set; }
        public Appointment() {}

        public Appointment(Company company, CompanyAdmin companyAdmin, DateTime startingDateTime, DateTime endingDateTime)
        {
            Id = Guid.NewGuid();
            CompanyAdmin = companyAdmin;
            StartingDateTime = startingDateTime;
            EndingDateTime = endingDateTime;
            Company = company;
        }
    }
}
