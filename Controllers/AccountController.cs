using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using githubtriggerbot.Models;
using githubtriggerbot.Models.AccountViewModels;
using githubtriggerbot.Services;

namespace githubtriggerbot.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly IEmailSender _emailSender;
        readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(
            string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return BuildRedirectionToExternalLoginError("Unknown");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Unable to retrieve the external login info";
                return BuildRedirectionToExternalLoginError(info.LoginProvider);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
                return RedirectToAction(nameof(Lockout));

            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            var user = new ApplicationUser
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                DisplayName = info.Principal.FindFirstValue("urn:github:name"),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                EmailConfirmed = true
            };

            var identityResult = await CreateExternalUser(info, user);
            if (identityResult.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return RedirectToLocal(returnUrl);
            }

            AddIdentityErrors(identityResult);
            return BuildRedirectionToExternalLoginError(info.LoginProvider);
        }

        IActionResult BuildRedirectionToExternalLoginError(string provider)
        {
            return View(
                "ExternalLoginError",
                 new ExternalLoginErrorViewModel { Provider = provider });
        }

        async Task<IdentityResult> CreateExternalUser(ExternalLoginInfo info, ApplicationUser externalUser)
        {
            var result = await _userManager.CreateAsync(externalUser);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(externalUser, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(externalUser, isPersistent: false);
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                    return result;
                }
            }
            return result;
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
