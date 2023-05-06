using Identity.API.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Identity.API.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext profileDataRequestContext)
        {
            var subject = profileDataRequestContext.Subject
                ?? throw new ArgumentNullException(nameof(profileDataRequestContext.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;

            var user = await _userManager.FindByNameAsync(subjectId);

            if (user == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var userClaims = GetClaimsFromUser(user);

            profileDataRequestContext.IssuedClaims = userClaims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext isActiveContext)
        {
            var subject = isActiveContext.Subject 
                ?? throw new ArgumentNullException(nameof(isActiveContext.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            isActiveContext.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var securityStamp = subject.Claims
                          .Where(c => c.Type == "security_stamp")
                          .Select(c => c.Value)
                          .SingleOrDefault();

                    if (securityStamp != null)
                    {
                        var dbSecurityStamp = await _userManager.GetSecurityStampAsync(user);

                        if (dbSecurityStamp != securityStamp)
                        {
                            return;
                        }
                    }

                    isActiveContext.IsActive =
                        !user.LockoutEnabled ||
                        !user.LockoutEnd.HasValue ||
                        user.LockoutEnd <= DateTime.Now;
                }
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, applicationUser.Id),
                new Claim(JwtClaimTypes.PreferredUserName, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.UserName)
            };

            // add more user details as customly named claims ...

            if (_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, applicationUser.Email),
                    new Claim(
                        JwtClaimTypes.EmailVerified, 
                        applicationUser.EmailConfirmed ? "true" : "false", 
                        ClaimValueTypes.Boolean
                    )
                });
            }

            return claims;  
        }
    }
}
