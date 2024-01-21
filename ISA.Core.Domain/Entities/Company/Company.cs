namespace ISA.Core.Domain.Entities.Company
{
    using ISA.Core.Domain.Entities.User;

    public class Company : Entity<Guid>
    {
        public string Name {  get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public int StartinWorkingHour { get; set; }
        public int EndWorkingHour { get; set; }
        public double? AverageGrade {  get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<CompanyAdmin>? Admins { get; set; }
        public List<Equipment>? Equipment { get; set; }



        public Company() {}


        public Company(string name, Address address, string description, int startWorkingHour, int endWorkingHour)
        {
            Id = new Guid();
            Name = name;
            Address = address;
            Description = description;
            StartinWorkingHour = startWorkingHour;
            EndWorkingHour = endWorkingHour;
        }


    }
}
