using Catalog.API.Data.Data.Models;
using Catalog.API.DTOs.Identity;
using Catalog.API.Utils;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Catalog.API.Services.Data.Interfaces
{
    public interface IIdentityService
    {
        IImmutableDictionary<string, RefreshTokenDTO> UsersRefreshTokensReadOnlyDictionary { get; }

        JWTAuthenticationResultDTO GenerateJWTToken(string userName, Claim[] claims);

        void RemoveExpiredRefreshTokens(DateTime currentDateTime);

        void RemoveRefreshTokensByUserName(string userName);

        JWTAuthenticationResultDTO Refresh(string refreshToken, string accessToken, DateTime currentDateTime);
    }
}
