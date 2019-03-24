using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ERPWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using ERPWebApp.Extensions;

namespace ERPWebApp.Controllers
{
    [Authorize] // kiểm tra đã đăng nhập với id chưa
   // [Route("[controller]/[action]")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var Namefull = User.GetSpecificClaim("FullName");
          
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
