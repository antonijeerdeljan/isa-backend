using ISA.Core.Domain.Entities.User;

namespace ISA.Application.API.Models.Requests;

public class CompanyRegisterModel
{
    public string Name { get; set; }
    public DateTime StartingWorkingHour { get; set; }
    public DateTime EndWorkingHour { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }

    public CompanyRegisterModel(string name, DateTime startingWorkingHour, DateTime endWorkingHour, string description, string country, string city, string street, int number)
    {
        Name = name;
        StartingWorkingHour = new DateTime(startingWorkingHour.Year, startingWorkingHour.Month, startingWorkingHour.Day, startingWorkingHour.Hour, startingWorkingHour.Minute, 0).ToUniversalTime();
        EndWorkingHour = new DateTime(endWorkingHour.Year, endWorkingHour.Month, endWorkingHour.Day, endWorkingHour.Hour, endWorkingHour.Minute, 0).ToUniversalTime();
        Description = description;
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }
}



