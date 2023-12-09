namespace ISA.Core.Domain.Entities.Company
{
    using ISA.Core.Domain.Entities.User;

    public class Company : Entity<Guid>
    {
        public string Name {  get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public double AverageGrade {  get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<User>? Admins { get; set; }

        public Guid AddresId { get; set; }



        public Company() 
        {
            Id = new Guid();
        }

        public Company(string name, Address address, string description, double averageGrade, List<Appointment> appointments, List<User> admins)
        {
            Id = new Guid();
            Name = name;
            Address = address;
            Description = description;
            AverageGrade = averageGrade;
            Appointments = appointments;
            Admins = admins;
            AddresId = address.Id;
        }


    }
}
