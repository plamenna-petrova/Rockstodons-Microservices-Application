using Catalog.API.DataModels;
using Catalog.API.DTOs;
using Catalog.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly CatalogDbContext catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> catalogSettings;

        public CatalogController(CatalogDbContext catalogContext, IOptionsSnapshot<CatalogSettings> catalogSettings)
        {
            this.catalogContext = catalogContext ?? 
                throw new ArgumentNullException(nameof(catalogContext));
            this.catalogSettings = catalogSettings;
            this.catalogContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("albums")]
        [ProducesResponseType(typeof(PaginatedItemDTO<Album>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Album>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAlbums([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var albumsToRetrieve = await GetAlbumsByIds(ids);

                if (!albumsToRetrieve.Any())
                {
                    return BadRequest("ids values are invalid. Must be comma-separated list of numbers");
                }

                return Ok(albumsToRetrieve);
            }

            var totalAlbums = await this.catalogContext.Albums.LongCountAsync();

            var albumsOnPage = await catalogContext.Albums
                .OrderBy(a => a.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var paginatedAlbumsListDTO = new PaginatedItemDTO<Album>(pageIndex, pageSize, totalAlbums, albumsOnPage);

            return Ok(paginatedAlbumsListDTO);
        }

        [HttpGet]
        [Route("album-types")]
        [ProducesResponseType(typeof(List<AlbumType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AlbumType>>> GetAlbumTypes()
        {
            return await this.catalogContext.AlbumTypes.ToListAsync();
        }

        [HttpGet]
        [Route("genres")]
        [ProducesResponseType(typeof(List<Genre>), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<List<Genre>>> GetGenres()
        {
            return await this.catalogContext.Genres.ToListAsync();  
        }

        private async Task<List<Album>> GetAlbumsByIds(string ids)
        {
            var idsNumbers = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (idsNumbers.All(idnum => idnum.Ok))
            {
                return new List<Album>();
            }

            var idsToSelect = idsNumbers.Select(id => id.Value);
            var albums = await catalogContext.Albums.Where(a => idsToSelect.Contains(a.Id)).ToListAsync();

            return albums;
        }
    }
}
