using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteHttp.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebsiteHttp.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(UserManager<User> userManager,SignInManager<User> signInManager)
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
                var errorMessage = CreatErrorMessage(signInResult);
                ModelState.AddModelError("",errorMessage);
                return View("Index", model);
            }

            return RedirectToAction("Index", "Home");
        }

        private string CreatErrorMessage(SignInResult signInResult)
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