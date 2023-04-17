using Catalog.API.DTOs.Identity;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Catalog.API.Services.Data.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, RefreshTokenDTO> _usersRefreshTokens;

        public IImmutableDictionary<string, RefreshTokenDTO> UsersRefreshTokensReadOnlyDictionary =>
            _usersRefreshTokens.ToImmutableDictionary();

        public IdentityService( IConfiguration configuration)
        {
            _configuration = configuration;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshTokenDTO>();
        }

        public JWTAuthenticationResultDTO GenerateJWTToken(string userName, Claim[] claims)
        {
            var currentDateTime = DateTime.UtcNow;

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWTConfiguration:Secret"])
            );

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWTConfiguration:Issuer"],
                audience: _configuration["JWTConfiguration:Audience"],
                claims: claims,
                notBefore: currentDateTime,
                expires: currentDateTime.AddMinutes(1),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var encryptedAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var generatedRefreshToken = new RefreshTokenDTO
            {
                UserName = userName,
                TokenString = GenerateRefreshTokenString(),
                ExpiredOn = currentDateTime.AddMinutes(5)
            };

            _usersRefreshTokens.AddOrUpdate(
                generatedRefreshToken.TokenString, 
                generatedRefreshToken, 
                (_, _) => generatedRefreshToken
            );

            return new JWTAuthenticationResultDTO
            {
                AccessToken = encryptedAccessToken,
                RefreshTokenDTO = generatedRefreshToken
            };
        }

        public void RemoveExpiredRefreshTokens(DateTime currentDateTime)
        {
            if (_usersRefreshTokens.Count > 0 )
            {
                var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpiredOn < currentDateTime).ToList();

                if (expiredTokens != null)
                {
                    foreach (var expiredToken in expiredTokens)
                    {
                        _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
                    }
                }
            }
        }

        public void RemoveRefreshTokensByUserName(string userName)
        {
            var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();

            foreach (var refreshToken in refreshTokens)
            {
                _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
            }
        }

        public JWTAuthenticationResultDTO Refresh(string refreshToken, string accessToken, DateTime currentDateTime)
        {
            var (principal, jwtToken) = DecodeJWTToken(accessToken);

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userName = principal.Identity?.Name;

            if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpiredOn < currentDateTime)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return GenerateJWTToken(userName, principal.Claims.ToArray());
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJWTToken(string tokenToDecode)
        {
            if (string.IsNullOrWhiteSpace(tokenToDecode))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(tokenToDecode,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["JWTConfiguration:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["JWTConfiguration:Secret"])),
                        ValidAudience = _configuration["JWTConfiguration:Audience"],
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
