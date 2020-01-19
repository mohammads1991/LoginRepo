using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteHttp.Models;

namespace WebsiteHttp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var errors = await AddUSerAsync(model);

            if (!string.IsNullOrEmpty(errors))
            {
                ModelState.AddModelError("",errors);
                return View("Index", model);
            }

            return RedirectToAction("Index","Home");
        }

       

        private async Task<string> AddUSerAsync(UserRegisterViewModel model)
        {
            var user=new User()
            {
                Id = model.UserName,
                Password = model.Password
            };
           var result=await _userManager.CreateAsync(user,user.Password);
           var errorBuilder = result.Errors.Aggregate(new StringBuilder(), (seed, error) =>
           {
               seed.Append((error.Description + Environment.NewLine));
               return seed;
           });

           return errorBuilder.ToString();

        }
    }

}