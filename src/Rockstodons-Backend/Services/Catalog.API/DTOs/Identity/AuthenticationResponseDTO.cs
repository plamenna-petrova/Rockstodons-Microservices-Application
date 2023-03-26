using Newtonsoft.Json;

namespace Catalog.API.DTOs.Identity
{
    public class AuthenticationResponseDTO
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
