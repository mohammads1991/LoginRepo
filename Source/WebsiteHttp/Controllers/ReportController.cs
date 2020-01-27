using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebsiteHttp.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}