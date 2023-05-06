using Duende.IdentityServer.Models;

namespace Identity.API.ViewModels.AccountViewModels
{
    public class ConsentViewModel : ConsentInputModel
    {
        public ConsentViewModel(
            ConsentInputModel consentInputModel, 
            string returnUrl, 
            Client client, 
            Resources resources
        )
        {
            RememberConsent = consentInputModel?.RememberConsent ?? true;
            ScopesConsented = consentInputModel?.ScopesConsented ?? Enumerable.Empty<string>();

            ReturnUrl = returnUrl;

            ClientName = client.ClientName;
            ClientUrl = client.ClientUri;
            ClientLogoUrl = client.LogoUri;
            AllowRememberConsent = client.AllowRememberConsent;

            IdentityScopes = resources.IdentityResources
                .Select(x => new ScopeViewModel(
                    x, ScopesConsented.Contains(x.Name) || consentInputModel == null)
                 )
                .ToArray();

            ApiScopes = resources.ApiScopes
                .Select(x => new ScopeViewModel(
                    x, ScopesConsented.Contains(x.Name) || consentInputModel == null)
                 )
                .ToArray();
        }

        public string ClientName { get; init; }

        public string ClientUrl { get; init; }

        public string ClientLogoUrl { get; init; }

        public bool AllowRememberConsent { get; init; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; init; }

        public IEnumerable<ScopeViewModel> ApiScopes { get; init; }
    }

    public record ScopeViewModel
    {
        public ScopeViewModel(ApiScope apiScope, bool check)
        {
            Name = apiScope.Name;
            DisplayName = apiScope.DisplayName;
            Description = apiScope.Description;
            Emphasize = apiScope.Emphasize;
            Required = apiScope.Required;
            Checked = check || apiScope.Required;
        }

        public ScopeViewModel(IdentityResource identityResource, bool check)
        {
            Name = identityResource.Name;
            DisplayName = identityResource.DisplayName; 
            Description = identityResource.Description;
            Emphasize = identityResource.Emphasize;
            Required = identityResource.Required;
            Checked = check || identityResource.Required;
        }

        public string Name { get; init; }
        
        public string DisplayName { get; init; }

        public string Description { get; init; }

        public bool Emphasize { get; init; }

        public bool Required { get; init; }

        public bool Checked { get; init; }
    }
}
