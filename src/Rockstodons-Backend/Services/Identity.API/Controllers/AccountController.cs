using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity.API.Models;
using Identity.API.Services.Interfaces;
using Identity.API.ViewModels.AccountViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Security.Claims;

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

            var buildedLoginViewModel = await BuildLoginViewModelAsync(returnUrl, authorizationContext!);

            ViewData["ReturnUrl"] = returnUrl;

            return View(buildedLoginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(loginViewModel.Email);

                if (await _loginService.ValidateCredentials(user, loginViewModel.Password))
                {
                    var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);

                    var authenticationProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true,
                        RedirectUri = loginViewModel.ReturnUrl
                    };

                    if (loginViewModel.RememberMe)
                    {
                        var permanentTokeLifetime = _configuration
                            .GetValue("PermanentTokenLifetimeDays", 365);

                        authenticationProperties.ExpiresUtc = DateTimeOffset.UtcNow
                            .AddDays(permanentTokeLifetime);
                        authenticationProperties.IsPersistent = true;
                    }

                    await _loginService.SignInAsync(user, authenticationProperties);

                    if (_identityServerInteractionService.IsValidReturnUrl(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            var buildedLoginViewModel = await BuildLoginViewModelAsync(loginViewModel);

            ViewData["ReturnUrl"] = loginViewModel.ReturnUrl;

            return View(buildedLoginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity!.IsAuthenticated == false)
            {
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var logoutContext = await _identityServerInteractionService
                .GetLogoutContextAsync(logoutId);

            if (logoutContext?.ShowSignoutPrompt == false)
            {
                return await Logout(new LogoutViewModel() { LogoutId = logoutId });
            }

            var logoutViewModel = new LogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(logoutViewModel);
        }

        public async Task<IActionResult> Logout(LogoutViewModel logoutViewModel)
        {
            var identityProvider = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (identityProvider != null && 
                identityProvider != IdentityServerConstants.LocalIdentityProvider)
            {
                if (logoutViewModel.LogoutId == null)
                {
                    logoutViewModel.LogoutId = 
                        await _identityServerInteractionService.CreateLogoutContextAsync();
                }

                string url = $"/Account/Logout?logoutId={logoutViewModel.LogoutId}";

                try
                {
                    await HttpContext.SignOutAsync(identityProvider, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception, 
                        "LOGOUT ERROR: {ExceptionMessage}", exception.Message
                    );
                }
            }

            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var logout = await _identityServerInteractionService
                .GetLogoutContextAsync(logoutViewModel.LogoutId);

            return Redirect(logout?.PostLogoutRedirectUri!);
        }

        public async Task<IActionResult> DeviceLogOut(string redirectUrl)
        {
            await HttpContext.SignOutAsync();

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null!)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel registerViewModel, string returnUrl = null!
        )
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                var createUserResult = await _userManager
                    .CreateAsync(applicationUser, registerViewModel.Password);

                if (createUserResult.Errors.Count() > 0)
                {
                    AddErrors(createUserResult);

                    return View(registerViewModel);
                }
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity!.IsAuthenticated)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("login", "account", new { returnUrl });
                    }
                    else
                    {
                        return View(registerViewModel);
                    }
                }
            }

            return null!;
        }

        [HttpGet]
        public IActionResult Redirecting()
        {
            return View();
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel loginViewModel)
        {
            var authorizationRequest = await _identityServerInteractionService
                .GetAuthorizationContextAsync(loginViewModel.ReturnUrl);

            var buildedLoginViewModel = await BuildLoginViewModelAsync(
                loginViewModel.ReturnUrl, authorizationRequest
            );
            buildedLoginViewModel.Email = loginViewModel.Email;
            buildedLoginViewModel.RememberMe = loginViewModel.RememberMe;

            return buildedLoginViewModel; 
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(
            string returnUrl, AuthorizationRequest authorizationRequest)
        {
            bool allowLocal = true;

            if (authorizationRequest?.Client != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(
                    authorizationRequest.Client.ClientId
                );

                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = authorizationRequest?.LoginHint!
            };
        }

        private void AddErrors(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
