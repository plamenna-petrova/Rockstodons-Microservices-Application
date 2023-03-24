using Catalog.API.Services.Mapping;

namespace Catalog.API.DTOs.AlbumTypes
{
    public class AlbumTypeDetailsDTO : IMapFrom<AlbumTypeDTO>
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
