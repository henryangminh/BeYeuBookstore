using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class AdvertiseContractController : Controller
    {
        IAdvertiserService _advertiserService;
        IWebMasterService _webMasterService;
        IAdvertisementContentService _advertisementContentService;
        IAdvertiseContractService _advertiseContractService;
        IAuthorizationService _authorizationService;
        IUnitOfWork _unitOfWork;
        public AdvertiseContractController(IAuthorizationService authorizationService ,IAdvertisementContentService advertisementContentService, IWebMasterService webMasterService,IAdvertiserService advertiserService,IAdvertiseContractService advertiseContractService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _advertisementContentService = advertisementContentService;
            _advertiserService = advertiserService;
            _webMasterService = webMasterService;
            _advertiseContractService = advertiseContractService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.AdvertiseContract, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
          
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var A = _advertiserService.GetBysId(userid);
            var WM = _webMasterService.GetBysId(userid);
            bool? isSaleAdmin = false;
            bool? isAccountant = false;
            if (WM != null)
            {
                if (WM.WebMasterTypeFK == Const_WebmasterType.Admin)
                {
                    isSaleAdmin = true;
                }
                if (WM.WebMasterTypeFK == Const_WebmasterType.Accountant)
                {
                    isAccountant = true;
                }
            }

            var model = _advertiseContractService.GetAllPaging(isSaleAdmin, isAccountant,A.KeyId, status, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertiseContractViewModel advertiseContractVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (advertiseContractVm.KeyId == 0)
                {

                    _advertiseContractService.Add(advertiseContractVm);
                }
                else
                {
                    _advertiseContractService.Update(advertiseContractVm);
                }
                _advertiseContractService.Save();
                return new OkObjectResult(advertiseContractVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllAdContentByAdvertiserId()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var A = _advertiserService.GetBysId(userid);
            var model = _advertisementContentService.GetAllCensoredAdContentByAdvertiserId(A.KeyId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllFutureSuccessContract()
        {
            var model = _advertiseContractService.GetAllFutureSuccessContract();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAdContentById(int id)
        {
            var model = _advertisementContentService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _advertiseContractService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAdvertiserInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var model = _advertiserService.GetBysId(userid);
            return new OkObjectResult(model);
        }


        [HttpPost]
        public IActionResult UpdateStatus(int id, int status, string note)
        {
            _advertiseContractService.UpdateStatus(id, status, note);
            _advertiseContractService.Save();
            return new OkObjectResult("true");
        }
    }
}