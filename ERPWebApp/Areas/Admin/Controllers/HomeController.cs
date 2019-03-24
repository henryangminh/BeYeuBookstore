using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var Namefull = User.GetSpecificClaim("FullName");
            return View();
        }
    }
}