using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class AdvertiseContractController : Controller
    {
        IAdvertiseContractService _advertiseContractService;
        IUnitOfWork _unitOfWork;
        public AdvertiseContractController(IAdvertiseContractService advertiseContractService, IUnitOfWork unitOfWork)
        {
            _advertiseContractService = advertiseContractService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}