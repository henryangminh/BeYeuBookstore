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

        [HttpGet]
        public IActionResult GetAllPaging(int type, Status status, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _webMasterService.GetAllPaging(type, status, fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }
    }
}