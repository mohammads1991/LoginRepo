using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Saml2;
using WebsiteHttp.Models;

namespace WebsiteHttp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;


        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var errors = AddUSerAsync(model).Result;

            if (!string.IsNullOrEmpty(errors))
            {
                ModelState.AddModelError("", errors);
                return View("Index", model);
            }


            var user = new User
            {
                Id = model.UserName
            };

            


            string confirmationToken = _userManager
                .GenerateEmailConfirmationTokenAsync(user).Result;

            

            string confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userid = user.Id, token = confirmationToken },
                protocol: HttpContext.Request.Scheme);


            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = @"C:\Test";

            client.Send("test@localhost", user.Id, "confirm your email", confirmationLink);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string userid, string token)
        {


            User user = _userManager.FindByIdAsync(userid).Result;

            IdentityResult result = _userManager.ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            {
                ViewBag.Message = "Email Confirmed Successfully!";
                return View();
            }
            else
            {
                ViewBag.Message = "Error!";
                return View();
            }

        }
        public IActionResult ResetPassword(string Token)
        {

            return View();
        }
        [HttpPost]

        public IActionResult ResetPassword(ResetPasswordViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", obj);
            }


            var user = _userManager.FindByIdAsync(obj.Email).Result;

            var result = _userManager.ResetPasswordAsync(user, ViewBag.Message, obj.Password).Result;
            if (!result.Succeeded)
            {
                ViewBag.Message = "Error while reseting the password";
                return View("ResetPassword", obj);
            }

            ViewBag.Message = "password reset successful";
            return RedirectToAction("Index", "Login");


        }


        private async Task<string> AddUSerAsync(UserRegisterViewModel model)
        {
            var user = new User()
            {
                Id = model.UserName,
                Password = model.Password
            };
            var result = await _userManager.CreateAsync(user, user.Password);
            var errorBuilder = result.Errors.Aggregate(new StringBuilder(), (seed, error) =>
            {
                seed.Append((error.Description + Environment.NewLine));
                return seed;
            });

            return errorBuilder.ToString();

        }
    }

}