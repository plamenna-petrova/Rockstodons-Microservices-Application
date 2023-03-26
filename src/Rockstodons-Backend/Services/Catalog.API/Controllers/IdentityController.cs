using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Identity;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

                var tokenClaims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userToAuthenticate.Id.ToString()),
                    new Claim(ClaimTypes.Email, userToAuthenticate.Email),
                    new Claim(ClaimTypes.Name, userToAuthenticate.UserName),
                    new Claim(ClaimTypes.Role, userRoles[0]),
                    new Claim(JwtRegisteredClaimNames.Sub, userToAuthenticate.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString())
                };

                var jwtAuthenticationResult = _identityService.GenerateJWTToken(
                    userToAuthenticate.UserName, tokenClaims
                );

                return new AuthenticationResponseDTO
                {
                    UserName = userToAuthenticate.UserName,
                    Role = userRoles[0],
                    AccessToken = jwtAuthenticationResult.AccessToken,
                    RefreshToken = jwtAuthenticationResult.RefreshTokenDTO.TokenString
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, "Register", exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("current-user")]
        [Authorize]
        public ActionResult GetCurrentUsser()
        {
            return Ok(new AuthenticationResponseDTO
            {
                UserName = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var userName = User.Identity?.Name;
            _identityService.RemoveRefreshTokensByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out of the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            try
            {
                var userName = User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh the JWT token.");

                if (string.IsNullOrWhiteSpace(refreshTokenRequestDTO.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtAuthenticationResult = _identityService.Refresh(refreshTokenRequestDTO.RefreshToken, accessToken, DateTime.Now);

                _logger.LogInformation($"User [{userName}] has refreshed the JWT token.");

                return Ok(new AuthenticationResponseDTO
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtAuthenticationResult.AccessToken,
                    RefreshToken = jwtAuthenticationResult.RefreshTokenDTO.TokenString
                });
            }
            catch (SecurityTokenException securityTokenException)
            {
                return Unauthorized(securityTokenException.Message);
            }
        }
    }
}
