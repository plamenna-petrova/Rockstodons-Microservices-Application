using System.Text.Json.Serialization;

namespace Catalog.API.DTOs.Identity
{
    public class RefreshTokenDTO
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("encryptedToken")]
        public string TokenString { get; set; }

        [JsonPropertyName("expiredOn")]
        public DateTime ExpiredOn { get; set; }
    }
}
