namespace ISA.Core.Domain.Entities.Company
{
    using System.ComponentModel;

    public class Appointment : Entity<Guid>
    {
        public string AdminFirstName {  get; set; }
        public string AdminLastName {  get; set; }
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public Guid AdminId { get; set; }
        public Guid CompanyId {  get; set; }

        public Appointment()
        {
            Id = Guid.NewGuid();
        }

        public Appointment(Guid companyId, Guid adminId ,string adminFirstName, string adminLastName, DateTime dateTime, int duration)
        {
            Id = Guid.NewGuid();
            AdminFirstName = adminFirstName;
            AdminLastName = adminLastName;
            DateTime = dateTime;
            Duration = duration;
            AdminId = adminId;
            CompanyId = companyId;
        }
    }
}
