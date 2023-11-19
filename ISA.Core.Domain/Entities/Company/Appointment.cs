namespace ISA.Core.Domain.Entities.Company
{
    public class Appointment : Entity<Guid>
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }

        public Guid CompanyId {  get; set; }

        public Appointment()
        {
        }
    }
}
