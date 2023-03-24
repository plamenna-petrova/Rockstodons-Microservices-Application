using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.AlbumTypes
{
    public class UpdateAlbumTypeDTO : IMapFrom<AlbumType>
    {
        public string Name { get; set; }
    }
}
