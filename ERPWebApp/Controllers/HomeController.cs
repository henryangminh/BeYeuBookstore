using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeYeuBookstore.Models;
using Microsoft.AspNetCore.Authorization;
using BeYeuBookstore.Extensions;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Utilities.Constants;

namespace BeYeuBookstore.Controllers
{
    [Authorize] // kiểm tra đã đăng nhập với id chưa
   // [Route("[controller]/[action]")]
    public class HomeController : BaseController
    {

        ICustomerService _customerService;
        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            var Namefull = User.GetSpecificClaim("FullName");
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var C = _customerService.GetBysId(userid);
            if(C.KeyId!=0)
            {
                return new RedirectResult("/Beyeubookstore");
            }
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
