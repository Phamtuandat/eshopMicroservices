using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity.Api.Areas.Identity.Models.Account.Login;
using Identity.Api.Areas.Identity.Models.Account.Register;
using Identity.Api.Enums;
using Identity.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using MassTransit;
using BuildingBlocks.Messaging.Events;
using Duende.IdentityServer.Extensions;
using Identity.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Api.Areas.Identity.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("Identity")]
    [Route("Account/[action]")]
    public class AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events, IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        IPublishEndpoint publish
        ) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IIdentityServerInteractionService _interaction = interaction;
        private readonly IEventService _events = events;
        private readonly IAuthenticationSchemeProvider _schemeProvider = schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore = identityProviderStore;
        private readonly ILogger<AccountController> _logger = logger;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IPublishEndpoint _publish = publish;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var viewModel = await BuildModelAsync(returnUrl);

            if (viewModel.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "ExternalLogin", new { scheme = viewModel.ExternalLoginScheme, returnUrl });
            }
            return View(new LoginInputModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(input.ReturnUrl);

            // the user clicked the "cancel" button
            if (input.Button != "login")
            {
                if (context != null)
                {
                    // This "can't happen", because if the ReturnUrl was null, then the context would be null
                    ArgumentNullException.ThrowIfNull(input.ReturnUrl, nameof(input.ReturnUrl));

                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);



                    return Redirect(input.ReturnUrl ?? "~/");
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(input.Username!, input.Password!, input.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(input.Username!);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user!.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                    Telemetry.Metrics.UserLogin(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider);

                    if (context != null)
                    {
                        // This "can't happen", because if the ReturnUrl was null, then the context would be null
                        ArgumentNullException.ThrowIfNull(input.ReturnUrl, nameof(input.ReturnUrl));



                        // we can trust input.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(input.ReturnUrl ?? "~/");
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(input.ReturnUrl))
                    {
                        return Redirect(input.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(input.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new ArgumentException("invalid return URL");
                    }
                }

                const string error = "invalid credentials";
                await _events.RaiseAsync(new UserLoginFailureEvent(input.Username, error, clientId: context?.Client.ClientId));
                Telemetry.Metrics.UserLoginFailure(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider, error);
                ModelState.AddModelError(error);
            }

            await BuildModelAsync(input.ReturnUrl);
            return View(input.ReturnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var viewModel = await BuildModelAsync(returnUrl);

            if (viewModel.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "ExternalLogin", new { scheme = viewModel.ExternalLoginScheme, returnUrl });
            }
            return View(new RegisterInputModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            var context = await _interaction.GetAuthorizationContextAsync(input.ReturnUrl);

            if (input.Button != "register")
            {
                if (context != null)
                {
                    // This "can't happen", because if the ReturnUrl was null, then the context would be null
                    ArgumentNullException.ThrowIfNull(input.ReturnUrl, nameof(input.ReturnUrl));

                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);



                    return Redirect(input.ReturnUrl ?? "~/");
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = input.Username, Email = input.Email, LastName = input.LastName, FirstName = input.FirstName };
                var result = await _userManager.CreateAsync(user, input.Password);
               
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, RoleNames.User);
                        await _signInManager.PasswordSignInAsync(input.Username!, input.Password!, input.RememberLogin, lockoutOnFailure: true);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user!.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                        Telemetry.Metrics.UserLogin(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider);
                        _logger.LogInformation($"User is created, {user.Email}");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var callbackUrl = Url.ActionLink(
                              action: nameof(EmailConfirmation),
                              values:
                                    new
                                    {
                                        area = "Identity",
                                        userId = user.Id,
                                        username = user.UserName,
                                        email = user.Email,
                                        code
                                    },
                              protocol: Request.Scheme);

                        _logger.LogInformation(callbackUrl);
                        if (!callbackUrl.IsNullOrEmpty())
                        {
                            var message = new RegiestedUserEvent()
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                ConfirmUrl = callbackUrl
                            };
                            await _publish.Publish(message);
                        }
                        else
                        {
                            _logger.LogInformation("can not create the email confirmation callback url");
                        }
                        if (context != null)
                        {
                            // This "can't happen", because if the ReturnUrl was null, then the context would be null
                            ArgumentNullException.ThrowIfNull(input.ReturnUrl, nameof(input.ReturnUrl));
                            // we can trust input.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(input.ReturnUrl ?? "~/");
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(input.ReturnUrl))
                        {
                            return Redirect(input.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(input.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new ArgumentException("invalid return URL");
                        }
                }

                const string error = "invalid credentials";
                await _events.RaiseAsync(new UserLoginFailureEvent(input.Username, error, clientId: context?.Client.ClientId));
                Telemetry.Metrics.UserLoginFailure(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider, error);
                ModelState.AddModelError(result);
            }

            // something went wrong, show form with error
            await BuildModelAsync(input.ReturnUrl);
            return View(input.ReturnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterConfirmation()
        {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation(string userId, string username, string email, string code)
        {
            if (userId == null || code == null)
            {
                return View("ErrorConfirmEmail");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ErrorConfirmEmail");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (_roleManager.FindByNameAsync(RoleNames.Administrator) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.User));
            }
            await _userManager.AddToRoleAsync(user, RoleNames.User);
            ViewBag.Email = email; 
            ViewBag.Username = username;
            return View();
        }

        [HttpPost("/logout/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User Logout");
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }


        private async Task<LoginViewModel> BuildModelAsync(string? returnUrl)
        {
            var inputModel = new LoginInputModel
            {
                ReturnUrl = returnUrl
            };

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var view = new LoginViewModel
                {
                    EnableLocalLogin = local,
                };

                inputModel.Username = context.LoginHint;

                if (!local)
                {
                    view.ExternalProviders = new[] { new LoginViewModel.ExternalProvider(authenticationScheme: context.IdP) };
                }

                return view;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new LoginViewModel.ExternalProvider
                (
                    authenticationScheme: x.Name,
                    displayName: x.DisplayName ?? x.Name
                )).ToList();

            var dynamicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
                .Where(x => x.Enabled)
                .Select(x => new LoginViewModel.ExternalProvider
                (
                    authenticationScheme: x.Scheme,
                    displayName: x.DisplayName ?? x.Scheme
                ));
            providers.AddRange(dynamicSchemes);

            var allowLocal = true;
            var client = context?.Client;
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Count != 0)
                {
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = LoginOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
                ExternalProviders = providers.ToArray()
            };
        }


    }
}
