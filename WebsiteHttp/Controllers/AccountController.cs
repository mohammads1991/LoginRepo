using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (CheckedUserExist(model.UserName))
            {
                ModelState.AddModelError("",$"this user :{model.UserName} exist");
                return View("Index", model);
            }


            return View();
        }

        private bool CheckedUserExist(string userName)
        {
            return _userManager.Users.Any(usr => usr.Id == userName);
        }
    }
}