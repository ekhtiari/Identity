using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Models;
using Identity.Models.User;
using Identity.Plugins;
using Indentity.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Identity.Controllers
{
    [Authorize(Roles = "admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, ApplicationDbContext _db, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            db = _db;

            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {

                //var user =await userManager.FindByNameAsync(model.UserName);

                //if (user.UserIdentity=="login")
                //{
                //    ViewBag.LoginError="This account already login . please try another account";
                //    return View(model);
                //}

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // HttpContext.User.Identities.FirstOrDefault().AddClaim(new System.Security.Claims.Claim("UserIdentity", user.UserIdentity ?? "login"));

                    //return RedirectToAction("MyOrder");
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                if (result.IsLockedOut)
                {
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //var userId = this.User.GetUserId();
            //var user =await userManager.FindByIdAsync(userId);

            //user.UserIdentity="";
            //db.SaveChanges();

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]




        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult MyOrder()
        {
            return RedirectToAction("Index", "Home");

        }

        #endregion
    }
}