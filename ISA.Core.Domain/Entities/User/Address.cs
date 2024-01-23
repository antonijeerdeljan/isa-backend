using System.Numerics;

namespace ISA.Core.Domain.Entities.User;

public class Address : Entity<Guid>
{
    public string Country { get; set; }
    public string City { get; set; }
    public Address()
    {
        Id = new Guid();
    }
    public Address(string country, string city)
    {
        Id = Guid.NewGuid();
        Country = country;
        City = city;
    }
}
