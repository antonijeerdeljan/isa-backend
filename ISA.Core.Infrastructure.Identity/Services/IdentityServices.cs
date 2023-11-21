using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Exceptions.UserExceptions;
using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.Entities.User;
using ISA.Core.Domain.Entities.Token;

namespace ISA.Core.Infrastructure.Identity.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public IdentityServices(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.Id = Guid.NewGuid();
            refreshToken.ExpirationDate = DateTime.UtcNow.AddDays(30);
            return refreshToken;
        }

        public async Task RegisterUserAsync(Guid id, string email, string password, string roleName)
        {
            ApplicationUser newUser = new ApplicationUser(id, email);

            IdentityResult registrationResult = await _userManager.CreateAsync(newUser, password);

            if (!registrationResult.Succeeded)
                throw new ArgumentException(registrationResult.ToString());

            try
            {
                await _userManager.AddToRoleAsync(newUser, roleName);
            }
            catch (Exception ex)
            {
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<LoginCookie> LoginAsync(string email, string password)
        {
            ApplicationUser? userToSignIn = await _userManager.FindByEmailAsync(email) ??
                throw new KeyNotFoundException("User with entered email does not exist!");

            var userRole = await _userManager.GetRolesAsync(userToSignIn);

            if (await _userManager.IsInRoleAsync(userToSignIn, userRole[0]) is false)
                throw new ArgumentException("Not allowed!");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(
                                                        user: userToSignIn,
                                                        password,
                                                        lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new ArgumentException(result.ToString());

            AuthenticationTokens token = new();
            LoginCookie loginCookie = new LoginCookie();

            try
            {
                token = _tokenGenerator.GenerateAccessToken(userToSignIn.Id.ToString(), userRole[0]);
                var refreshToken = GenerateRefreshToken();
                userToSignIn.RefreshToken = refreshToken.Id;
                userToSignIn.RefreshTokenExpirationDate = refreshToken.ExpirationDate;
                await _userManager.UpdateAsync(userToSignIn);
                loginCookie.AuthToken = token;
                loginCookie.RefreshToken = refreshToken;
            }
            catch(Exception ex)
            {
                
            }
            return loginCookie;

        }

       
    }
}
