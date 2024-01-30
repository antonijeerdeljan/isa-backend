namespace ISA.Core.Domain.Dtos.Customer
{
    using ISA.Core.Domain.Entities.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserProfileDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public AddressDto Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
