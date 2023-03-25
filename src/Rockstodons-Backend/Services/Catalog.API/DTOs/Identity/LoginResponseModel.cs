namespace Catalog.API.DTOs.Identity
{
    public class LoginResponseModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
