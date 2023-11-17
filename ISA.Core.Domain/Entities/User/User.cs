namespace ISA.Core.Domain.Entities.User;

public class User : Entity<Guid>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public User()
    {

    }
    public User(string firstname, string lastname, Address address, string email, string phoneNumber, DateTime dateOfBirth)
    {
        Firstname = firstname;
        Lastname = lastname;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
    }
}
