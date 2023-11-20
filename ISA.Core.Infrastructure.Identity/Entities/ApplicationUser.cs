using Microsoft.AspNetCore.Identity;

namespace ISA.Core.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ApplicationUser(Guid id, string email)
        {
            Id = id;
            Email = email;
            UserName = email;
            EmailConfirmed = false;
        }

        public ApplicationUser()
        {

        }
    }
}