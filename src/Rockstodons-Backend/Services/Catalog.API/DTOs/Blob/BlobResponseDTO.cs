namespace Catalog.API.DTOs.Blob
{
    public class BlobResponseDTO
    {
        public string? Status { get; set; }

        public bool Error { get; set; }

        public BlobDTO BlobDTO { get; set; }

        public BlobResponseDTO()
        {
            BlobDTO = new BlobDTO();
        }
    }
}
