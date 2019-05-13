using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class MerchantContractController : Controller
    {
        IMerchantContractService _merchantContractService;
        IMerchantService _merchantService;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public MerchantContractController(IMerchantService merchantService,IAuthorizationService authorizationService ,IMerchantContractService merchantContractService, IUnitOfWork unitOfWork)
        {
            _merchantService = merchantService;
            _authorizationService = authorizationService;
            _merchantContractService = merchantContractService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.MerchantContract, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging( string fromdate, string todate, string keyword, int page, int pageSize)
        {

            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _merchantContractService.GetAllPaging(M.KeyId, fromdate, todate, keyword, page, pageSize);
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