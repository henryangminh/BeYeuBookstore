using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    
    public class MerchantController : Controller
    {
        IMerchantService _merchantService;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public MerchantController(IAuthorizationService authorizationService, IMerchantService merchantService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _merchantService = merchantService;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.Merchant, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? status, int? scale, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _merchantService.GetAllPaging(status, scale, fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _merchantService.GetAll();
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