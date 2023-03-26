using Catalog.API.Common;
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
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IIdentityService identityService,
            IUsersService usersService,
            IConfiguration configuration,
            ILogger<IdentityController> logger
        )
        {
            _userManager = userManager;
            _identityService = identityService;
            _usersService = usersService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                var allUsers = await _usersService.GetAllUsers();
                var existingUser = allUsers.FirstOrDefault(u => u.UserName == registerRequestDTO.UserName
                    || u.Email == registerRequestDTO.Email);

                if (existingUser?.UserName == registerRequestDTO.UserName)
                {
                    throw new Exception($"The username is already taken: {registerRequestDTO.UserName}");
                }

                if (existingUser?.Email == registerRequestDTO.Email)
                {
                    throw new Exception($"The email is already taken: {registerRequestDTO.Email}");
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = registerRequestDTO.UserName,
                    Email = registerRequestDTO.Email
                };

                var userRegistrationResult = await _userManager.CreateAsync(applicationUser, registerRequestDTO.Password);

                if (!userRegistrationResult.Succeeded)
                {
                    return BadRequest(userRegistrationResult.Errors);
                }

                await _userManager.AddToRoleAsync(applicationUser, GlobalConstants.NormalUserRoleName);

                return Ok();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, "Register", exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<AuthenticationResponseDTO>> Authenticate(AuthenticationRequestDTO authenticationRequestDTO)
        {
            try
            {
                var userToAuthenticate = await _userManager.FindByNameAsync(authenticationRequestDTO.UserName);

                if (userToAuthenticate == null)
                {
                    return Unauthorized();
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(userToAuthenticate, authenticationRequestDTO.Password);

                if (!isPasswordValid)
                {
                    return Unauthorized();
                }

                var userRoles = await _userManager.GetRolesAsync(userToAuthenticate);
                var generatedToken = _identityService.GenerateJWTToken(userToAuthenticate, userRoles);

                return new AuthenticationResponseDTO
                {
                    Id = userToAuthenticate.Id,
                    UserName = userToAuthenticate.UserName,
                    Role = userRoles[0],
                    Token = generatedToken
                };
            }
            catch (Exception exception )
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, "Register", exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
