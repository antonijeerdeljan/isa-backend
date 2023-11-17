using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using ISA.Core.Domain.Contracts;

namespace ISA.Core.Infrastructure.Identity.Services;

public class IdentityServices : IIdentityServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    public IdentityServices(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterAsync(Guid id, string email, string password)
    {
        ApplicationUser newUser = new(id, email);
        IdentityResult registrationResult = await _userManager.CreateAsync(newUser, password);
        if (!registrationResult.Succeeded)
        {
            throw new ArgumentException(registrationResult.ToString());
        }
        await _userManager.AddToRoleAsync(newUser, IdentityRoles.REGISTERED);
    }

}
