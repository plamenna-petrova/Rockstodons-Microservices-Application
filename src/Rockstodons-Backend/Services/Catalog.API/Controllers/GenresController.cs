using Catalog.API.Data.Models;
using Catalog.API.DTOs.Genres;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService genresService;

        public GenresController(IGenresService genresService)
        {
            this.genresService = genresService;
        }

        [HttpGet]
        [Route("genres")]
        [ProducesResponseType(typeof(List<GenreDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            return await this.genresService.GetAllGenres<GenreDTO>();
        }
    }
}
