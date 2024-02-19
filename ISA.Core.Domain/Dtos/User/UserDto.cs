using ISA.Core.Domain.Entities.User;

namespace ISA.Core.Domain.Dtos.User;

public class UserDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}
