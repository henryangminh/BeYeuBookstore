using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class AdvertiseContractController : Controller
    {
        IAdvertiserService _advertiserService;
        IWebMasterService _webMasterService;
        IAdvertisementContentService _advertisementContentService;
        IAdvertiseContractService _advertiseContractService;
        IUnitOfWork _unitOfWork;
        public AdvertiseContractController(IAdvertisementContentService advertisementContentService, IWebMasterService webMasterService,IAdvertiserService advertiserService,IAdvertiseContractService advertiseContractService, IUnitOfWork unitOfWork)
        {
            _advertisementContentService = advertisementContentService;
            _advertiserService = advertiserService;
            _webMasterService = webMasterService;
            _advertiseContractService = advertiseContractService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var A = _advertiserService.GetBysId(userid);
            var WM = _webMasterService.GetBysId(userid);
            var model = _advertiseContractService.GetAllPaging(A.KeyId, status, keyword, page, pageSize);
            return new OkObjectResult(model);
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
        public IActionResult GetAdContentById(int id)
        {
            var model = _advertisementContentService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetAdvertiserInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var model = _advertiserService.GetBysId(userid);
            return new OkObjectResult(model);
        }
    }
}