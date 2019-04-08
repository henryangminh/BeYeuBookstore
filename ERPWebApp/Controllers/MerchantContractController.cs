using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class MerchantContractController : Controller
    {
        IMerchantContractService _merchantContractService;
        IUnitOfWork _unitOfWork;
        public MerchantContractController(IMerchantContractService merchantContractService, IUnitOfWork unitOfWork)
        {
            _merchantContractService = merchantContractService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}