using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class MerchantController : Controller
    {
        IMerchantService _merchantService;
        IUnitOfWork _unitOfWork;
        public MerchantController(IMerchantService merchantService, IUnitOfWork unitOfWork)
        {
            _merchantService = merchantService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}