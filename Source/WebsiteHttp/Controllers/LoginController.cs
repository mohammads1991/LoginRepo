using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WebsiteHttp.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebsiteHttp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        


        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> LoginAsync(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }



            var user = await _userManager.FindByIdAsync(model.Email);


            if (user==null)
            {
                ModelState.AddModelError("",$"User {model.Email} Is Not Exists");
                return View("Index", model);
            }

            
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (!signInResult.Succeeded)
            {
                var errorMessage = CreateErrorMessage(signInResult);
                ModelState.AddModelError("",errorMessage);
                return View("Index", model);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendPasswordResetLinkAsync(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgetPassword", model);
            }

            var user = await _userManager.FindByIdAsync(model.Email);

            if (user==null)
            {
                ModelState.AddModelError("",$"There Is No User With This Email {model.Email}");
                return View("ForgetPassword", model);
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;


            var resetLink = Url.Action("ResetPassword", "Account", new { token = token },
                HttpContext.Request.Scheme);
            ViewBag.Message = "Password reset link has sent to your email";
            ViewBag.Message = token;
            return RedirectToAction("ResetPassword", "Account");
            
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            

            return RedirectToAction("Index", "Home");
        }

        private string CreateErrorMessage(SignInResult signInResult)
        {
            if (signInResult.IsLockedOut)
            {
                return "User Is Locked";
            }

            if (signInResult.IsNotAllowed)
            {
                return "User Is Not Allowed";
            }

            return "User Can Not Login";
        }
    }
}