using System.Text.Json.Serialization;

namespace Catalog.API.DTOs.Identity
{
    public class JWTAuthenticationResultDTO
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public RefreshTokenDTO RefreshTokenDTO { get; set; }
    }
}
