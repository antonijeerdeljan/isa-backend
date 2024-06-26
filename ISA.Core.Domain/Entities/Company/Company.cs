﻿namespace ISA.Core.Domain.Entities.Company
{
    using ISA.Core.Domain.Entities.User;

    public class Company : Entity<Guid>
    {
        public string Name {  get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public TimeOnly StartingWorkingHour { get; set; }
        public TimeOnly EndWorkingHour { get; set; }
        public double? AverageGrade { get; set; } = 0;
        public List<Appointment>? Appointments { get; set; }
        public List<CompanyAdmin>? Admins { get; set; }
        public List<Equipment>? Equipment { get; set; }

        public List<Grade>? Grades {  get; set; }



        public Company() {}


        public Company(string name, Address address, string description, DateTime startWorkingHour, DateTime endWorkingHour)
        {
            Id = new Guid();
            Name = name;
            Address = address;
            Description = description;
            StartingWorkingHour = new TimeOnly(startWorkingHour.Hour, startWorkingHour.Minute);
            EndWorkingHour = new TimeOnly(endWorkingHour.Hour, endWorkingHour.Minute);
        }


    }
}
