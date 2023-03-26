using Catalog.API.Common;
using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.DTOs.Albums;
using Catalog.API.Services.Data.Implementation;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private const string RolesName = "Roles";
        private const string SingleRoleName = "role";
        private const string RoleDetailsRouteName = "RoleDetails";

        private readonly IRolesService _rolesService;
        private ILogger<RolesController> _logger;

        public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
        {
            _rolesService = rolesService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationRole>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ApplicationRole>>> GetAllRoles()
        {
            try
            {
                var allRoles = await _rolesService.GetAllRoles();

                if (allRoles != null)
                {
                    return Ok(allRoles);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, RolesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, RolesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    string.Format(GlobalConstants.GetAllEntitiesExceptionMessage, RolesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ApplicationRole>>> GetRolesWithDeletedRecords()
        {
            try
            {
                var allRolesWithDeletedRecords = await _rolesService.GetAllRolesWithDeletedRecords();

                if (allRolesWithDeletedRecords != null) 
                {
                    return Ok(allRolesWithDeletedRecords);
                }

                _logger.LogError(string.Format(GlobalConstants.EntitiesNotFoundResult, RolesName));

                return NotFound(string.Format(GlobalConstants.EntitiesNotFoundResult, RolesName));
            }
            catch (Exception exception)
            {
                _logger.LogError(string.Format(
                    GlobalConstants.GetAllEntitiesWithDeletedRecordsExceptionMessage, RolesName, exception.Message)
                );

                return StatusCode(StatusCodes.Status500InternalServerError, GlobalConstants.InternalServerErrorMessage);
            }
        }
    }
}
