using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Exceptions.UserExceptions;

namespace ISA.Core.Infrastructure.Identity.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        //private readonly RoleManager<IdentityRole> _roleManager; // Use IdentityRole here

        public IdentityServices(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task RegisterUserAsync(Guid id, string email, string password, string roleName)
        {
            ApplicationUser newUser = new ApplicationUser(id, email);

            IdentityResult registrationResult = await _userManager.CreateAsync(newUser, password);
            if (!registrationResult.Succeeded)
            {
                throw new ArgumentException(registrationResult.ToString());
            }

            try
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

                await _userManager.AddToRoleAsync(newUser, roleName);
            }
            catch (Exception ex)
            {
                throw new RegistrationException(ex.Message, ex);
            }
        }

    }
}
