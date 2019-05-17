using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class ProcessController : Controller
    {
        public IActionResult Advertiser()
        {
            return View();
        }
        public IActionResult Merchant()
        {
            return View();
        }
        public IActionResult GeneralTerm()
        {
            return View();
        }
        public IActionResult Address()
        {
            return View();
        }
        public IActionResult BankingInfo()
        {
            return View();
        }
    }
}