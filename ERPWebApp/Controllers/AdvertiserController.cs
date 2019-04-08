using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class AdvertiserController : Controller
    {
        IAdvertiserService _advertiserService;
        IUnitOfWork _unitOfWork;
        public AdvertiserController(IAdvertiserService advertiserService, IUnitOfWork unitOfWork)
        {
            _advertiserService = advertiserService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}