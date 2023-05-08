using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity.API.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class ConsentController : Controller
    {
        private readonly ILogger<ConsentController> _logger;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentController(
            ILogger<ConsentController> logger,
            IIdentityServerInteractionService identityServerInteractionService,
            IClientStore clientStore,
            IResourceStore resourceStore
        )
        {
            _logger = logger;
            _identityServerInteractionService = identityServerInteractionService;
            _clientStore = clientStore;
            _resourceStore = resourceStore; 
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            return null!;
        }

        async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel consentInputModel)
        {
            var authorizationRequest = await _identityServerInteractionService
                .GetAuthorizationContextAsync(returnUrl);

            if (authorizationRequest != null)
            {
                var client = await _clientStore
                    .FindEnabledClientByIdAsync(authorizationRequest.Client.ClientId);

                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(
                        authorizationRequest.Client.AllowedScopes);

                    if (resources != null && (resources.IdentityResources.Any() 
                        || resources.ApiResources.Any()))
                    {
                        return new ConsentViewModel(
                            consentInputModel, 
                            returnUrl, 
                            authorizationRequest, 
                            client, 
                            resources
                        );
                    }

                }
            }

            return null;
        }
    }
}
