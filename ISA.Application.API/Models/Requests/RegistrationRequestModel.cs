using ISA.Core.Domain.Entities.User;

namespace ISA.Application.API.Models.Requests
{
    public class RegistrationRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }

    }
}
