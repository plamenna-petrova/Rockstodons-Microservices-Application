namespace Catalog.API.Services.Data.Interfaces
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string userName, string role, string secret);
    }
}
