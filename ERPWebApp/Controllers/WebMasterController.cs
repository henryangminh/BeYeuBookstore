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
    [Authorize]
    public class WebMasterController : Controller
    {
        IWebMasterService _webMasterService;
        IWebMasterTypeService _webMasterTypeService;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public WebMasterController(IAuthorizationService authorizationService,IWebMasterTypeService webMasterTypeService, IWebMasterService webMasterService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _webMasterService = webMasterService;
            _webMasterTypeService = webMasterTypeService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.WebMaster, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? type, int? status, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _webMasterService.GetAllPaging(type, status, fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllWebMasterPosition()
        {
            var model = _webMasterTypeService.GetAll();
            return new OkObjectResult(model);
        }
    }
}