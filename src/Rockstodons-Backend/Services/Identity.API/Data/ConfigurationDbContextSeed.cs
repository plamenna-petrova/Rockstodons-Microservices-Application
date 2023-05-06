using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.API.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class ConfigurationDbContextSeed
    {
        public async Task SeedAsync(ConfigurationDbContext configurationDbContext, IConfiguration configuration)
        {
            var clientUrls = new Dictionary<string, string>
            {
                { "SPA", configuration.GetValue<string>("SPAClient") }
            };

            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in Config.GetClients(clientUrls))
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }

                await configurationDbContext.SaveChangesAsync();
            }
            else
            {
                List<ClientRedirectUri> oldRedirects =
                    (await configurationDbContext.Clients.Include(c => c.RedirectUris).ToListAsync())
                    .SelectMany(c => c.RedirectUris)
                    .Where(ru => ru.RedirectUri.EndsWith("/o2c.html"))
                    .ToList();

                if (oldRedirects.Any())
                {
                    foreach (var oldRedirect in oldRedirects)
                    {
                        oldRedirect.RedirectUri = oldRedirect.RedirectUri
                            .Replace("/o2c.html", "/oauth2-redirect.html");
                        configurationDbContext.Update(oldRedirect.Client);
                    }

                    await configurationDbContext.SaveChangesAsync();
                }
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in Config.GetResources())
                {
                    configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }

                await configurationDbContext.SaveChangesAsync();
            }
        }
    }
}
