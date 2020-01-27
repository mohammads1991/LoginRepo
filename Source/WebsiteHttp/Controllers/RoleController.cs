using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteHttp.Models;

namespace WebsiteHttp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Role> roleManager,UserManager<User> userManager)
        {
            this._roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Select(r => new RoleViewModel() { Name = r.Id }).ToList();

            return View(roles);
        }

        public IActionResult CreateRoles()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateRoles", model);
            }

            var role=new Role()
            {
                Id = model.Name
            };
            await _roleManager.CreateAsync(role);

            return RedirectToAction("Index");
        }

        public IActionResult AssignUsers()
        {
            var roles = _roleManager.Roles.ToList();
            var users = _userManager.Users.ToList();

            ViewBag.Roles = roles;
            ViewBag.Users = users;
            return View();
        }

        public async Task<IActionResult> AssignUser(UserToRoleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("AssignUsers", model);
            }

            var role =await _roleManager.FindByIdAsync(model.RoleName);
            if (role==null)
            {
                ModelState.AddModelError("",$"this role : {model.RoleName} is not valid");
                return View("AssignUsers", model);
            }

            var user =await _userManager.FindByIdAsync(model.UserName);
            if (user==null)
            {
                ModelState.AddModelError("",$"this user : {model.RoleName} is not valid");
                return View("AssignUsers", model); 
            }

            role.Users.Add(user);
            await _roleManager.UpdateAsync(role);


            return RedirectToAction("Index");
        }
    }
}