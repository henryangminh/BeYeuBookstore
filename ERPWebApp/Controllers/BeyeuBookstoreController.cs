using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class BeyeuBookstoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}