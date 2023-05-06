using Identity.API.Models;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services.Implementation
{
    public class LoginService : ILoginService<ApplicationUser>
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _userManager.FindByEmailAsync(user);
        }

        public async Task<bool> ValidateCredentials(ApplicationUser applicationUser, string password)
        {
            return await _userManager.CheckPasswordAsync(applicationUser, password);
        }

        public Task SignIn(ApplicationUser applicationUser)
        {
            return _signInManager.SignInAsync(applicationUser, true);
        }

        public Task SignInAsync(
            ApplicationUser applicationUser, 
            AuthenticationProperties authenticationProperties, 
            string authenticationMethod = null!
        )
        {
            return _signInManager.SignInAsync(applicationUser, authenticationProperties, authenticationMethod);
        }
    }
}
