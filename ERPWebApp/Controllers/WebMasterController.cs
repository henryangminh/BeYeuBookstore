using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class WebMasterController : Controller
    {
        IWebMasterService _webMasterService;
        IUnitOfWork _unitOfWork;
        public WebMasterController(IWebMasterService webMasterService, IUnitOfWork unitOfWork)
        {
            _webMasterService = webMasterService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}