using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            }; 
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "angular",
                    ClientName = "Rockstodons SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientsUrl["SPA"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientsUrl["SPA"]}/" },
                    AllowedCorsOrigins = { $"{clientsUrl["SPA"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}
