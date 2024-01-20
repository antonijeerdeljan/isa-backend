using ISA.Core.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using ISA.Core.Domain.Contracts;
using ISA.Core.Domain.Exceptions.UserExceptions;
using ISA.Core.Domain.Entities.Token;
using FluentResults;

namespace ISA.Core.Infrastructure.Identity.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHttpClientService _httpClientService;

        public IdentityServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenGenerator tokenGenerator,
            IHttpClientService httpClientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _httpClientService = httpClientService;
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            await _httpClientService.SendEmail(email, token);

            try
            {
                await _userManager.AddToRoleAsync(newUser, roleName);
            }
            catch (Exception ex)
            {
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<bool> VerifyRefreshToken(string userId, string refreshToken)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return (user.RefreshToken == Guid.Parse(refreshToken)) ? true : false;
        }

        public AuthenticationTokens GenerateNewJWT(string userId, string userRole)
        {
            return _tokenGenerator.GenerateAccessToken(userId, userRole);
        }

        public async Task VerifyEmail(string email, string token)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            Result<IdentityResult> confirmation = await _userManager.ConfirmEmailAsync(user, token);
            await _userManager.UpdateAsync(user);
        }


        private async Task<(ApplicationUser, string)> ValidateLogin(string email, string password)
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

            await _signInManager.SignInAsync(userToSignIn, false);

            return (userToSignIn, userRole[0]);
        }

        public async Task<LoginCookie> LoginAsync(string email, string password)
        {
            (ApplicationUser userToSignIn, string userRole) = await ValidateLogin(email, password);

            AuthenticationTokens token = new();
            LoginCookie loginCookie = new LoginCookie();

            token = _tokenGenerator.GenerateAccessToken(userToSignIn.Id.ToString(), userRole);
            var refreshToken = GenerateRefreshToken();
            userToSignIn.RefreshToken = refreshToken.Id;
            userToSignIn.RefreshTokenExpirationDate = refreshToken.ExpirationDate;

            try
            {
                await _userManager.UpdateAsync(userToSignIn);
                loginCookie.AuthToken = token;
                loginCookie.RefreshToken = refreshToken;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Unable to login");
            }
            loginCookie.AuthToken = token;
            loginCookie.RefreshToken = refreshToken;
            return loginCookie;

        }

        
    }
}
