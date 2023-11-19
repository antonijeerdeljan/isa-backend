namespace ISA.Application.API.Models.Requests;

public class EditProfileRequestModel
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public EditProfileRequestModel()
    {

    }

    public EditProfileRequestModel(string? name, string? lastname, string? phoneNumber, DateTime? dateOfBirth)
    {
        Name = name;
        Lastname = lastname;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
    }
}
