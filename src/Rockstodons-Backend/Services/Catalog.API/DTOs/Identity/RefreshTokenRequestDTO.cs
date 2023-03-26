using Newtonsoft.Json;

namespace Catalog.API.DTOs.Identity
{
    public class RefreshTokenRequestDTO
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
