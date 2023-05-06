using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity.API.Models;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
           ILoginService<ApplicationUser> loginService,
           IIdentityServerInteractionService identityServerInteractionService,
           IClientStore clientStore,
           ILogger<AccountController> logger,
           UserManager<ApplicationUser> userManager,
           IConfiguration configuration
        )
        {
            _loginService = loginService;
            _identityServerInteractionService = identityServerInteractionService;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var authorizationContext = await _identityServerInteractionService
                .GetAuthorizationContextAsync(returnUrl);

            if (authorizationContext?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            return null!;
        }
    }
}
