namespace ISA.Core.Domain.Dtos
{
    public class CorpAdminRegistrationDto 
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CompanyId { get; set; }       
    }
}
