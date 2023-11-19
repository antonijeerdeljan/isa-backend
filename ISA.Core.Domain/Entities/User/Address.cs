using System.Numerics;

namespace ISA.Core.Domain.Entities.User;

public class Address : Entity<Guid>
{
    public string Country { get; set; }
    public string City { get; set; }
    public Address()
    {

    }
    public Address(string country, string city)
    {
        Country = country;
        City = city;
    }
}
