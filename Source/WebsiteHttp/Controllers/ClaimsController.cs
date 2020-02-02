using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteHttp.Models;

namespace WebsiteHttp.Controllers
{
    [Authorize(Roles="MANAGER")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ClaimsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(CreateClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.FindByIdAsync(model.User);

            if (user==null)
            {
                ModelState.AddModelError("",$"this user {model.User} is not valid");
                return View("Index", model); 
            }

            var claim=new Claim(model.Type,model.Value);

           await _userManager.AddClaimAsync(user, claim);

           return RedirectToAction("Index", "Home");



        }
    }
}