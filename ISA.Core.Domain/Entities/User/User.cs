namespace ISA.Core.Domain.Entities.User;

public class User : Entity<Guid>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    //public Guid AddresId { get; set; }

    public Guid? CompanyId {  get; set; }

    public DateTime DateOfBirth { get; set;}

    public User()
    {

    }
    public User(Guid id,string firstname, string lastname, Address address, string email, string phoneNumber, DateTime dateOfBirth)
    {
        Id = id; 
        Firstname = firstname;
        Lastname = lastname;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
    }

    public User(Guid id, string firstname, string lastname, Address address, string email, string phoneNumber, Guid? companyId, DateTime dateOfBirth)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        CompanyId = companyId;
        DateOfBirth = dateOfBirth;
    }

    public void Update(string? name, string? lastname, string? phoneNumber, DateTime? dateOfBirth)
    {
        if (name != null)
            Firstname = name;

        if (lastname != null)
            Lastname = lastname;

        if (phoneNumber != null)
            PhoneNumber = phoneNumber;

        if (dateOfBirth != null)
            DateOfBirth = (DateTime)dateOfBirth;
    }
}
