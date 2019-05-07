using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult GetAllPaging( string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _merchantContractService.GetAllPaging(fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _merchantContractService.GetById(id);
            return new OkObjectResult(model);
        }
    }
}