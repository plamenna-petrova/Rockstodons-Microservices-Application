using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Identity;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IIdentityService identityService,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _identityService = identityService;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestModel registerRequestModel)
        {
            var userToCreate = new ApplicationUser
            {
                Email = registerRequestModel.Email,
                UserName = registerRequestModel.UserName,
            };

            var registrationResult = await _userManager.CreateAsync(userToCreate, registerRequestModel.Password);

            return Ok();
        }
    }
}
