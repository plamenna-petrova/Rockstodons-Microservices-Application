using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Implementation;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const string UsersName = "Users";
        private const string SingleUserName = "user";
        private const string UserDetailsRouteName = "UserDetails";

        private readonly IUsersService _usersService;
        private ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationUser>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
        {
            try
            {
                var allUsers = await _usersService.GetAllUsers();

                if (allUsers != null)
                {
                    return Ok(allUsers);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, UsersName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, UsersName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, UsersName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpDelete]
        [Route("confirm-deletion/{id}")]
        public async Task<ActionResult> HardDeleteUser(string id)
        {
            try
            {
                var userToHardDelete = await _usersService.GetUserById(id);

                if (userToHardDelete == null)
                {
                    _logger.LogError(string.Format(GlobalConstants.EntityByIdNotFoundResult, UsersName));

                    return NotFound(string.Format(GlobalConstants.EntityByIdNotFoundResult, UsersName));
                }

                await _usersService.HardDeleteUser(userToHardDelete);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                   string.Format(GlobalConstants.EntityHardDeletionExceptionMessage, SingleUserName, id, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
