using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Data.Enums;
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

        [HttpGet]
        public IActionResult GetAllPaging(Status status, Scale scale, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _merchantService.GetAllPaging(status, scale, fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _merchantService.GetById(id);
            return new OkObjectResult(model);
        }
    }
}