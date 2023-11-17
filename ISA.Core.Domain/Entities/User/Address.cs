using System.Numerics;

namespace ISA.Core.Domain.Entities.User;

public class Address
{
    public Guid Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public int ZipCode { get; set; }
    public Address()
    {

    }
    public Address(string country, string city, string street, int number, int zipCode)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
        ZipCode = zipCode;
    }
}
