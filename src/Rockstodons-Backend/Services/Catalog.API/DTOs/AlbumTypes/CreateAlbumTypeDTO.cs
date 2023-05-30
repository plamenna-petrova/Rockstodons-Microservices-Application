using Catalog.API.Data.Models;
using Catalog.API.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.AlbumTypes
{
    public class CreateAlbumTypeDTO : IMapFrom<AlbumType>
    {
        public string Name { get; set; }
    }
}
