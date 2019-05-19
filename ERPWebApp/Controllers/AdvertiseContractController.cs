using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class AdvertiseContractController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        IAdvertiserService _advertiserService;
        IAdvertisementPositionService _advertisementPositionService;
        IWebMasterService _webMasterService;
        IAdvertisementContentService _advertisementContentService;
        IAdvertiseContractService _advertiseContractService;
        IAuthorizationService _authorizationService;
        IUnitOfWork _unitOfWork;
        public AdvertiseContractController(IHostingEnvironment hostingEnvironment,IAdvertisementPositionService advertisementPositionService, IAuthorizationService authorizationService ,IAdvertisementContentService advertisementContentService, IWebMasterService webMasterService,IAdvertiserService advertiserService,IAdvertiseContractService advertiseContractService, IUnitOfWork unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
            _advertisementPositionService = advertisementPositionService;
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
        public IActionResult Statistic()
        {

            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.AdStatistic, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
          
        }


        [HttpGet]
        public IActionResult GetAllAdPosition()
        {
            var model = _advertisementPositionService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string fromdate, string todate, int? status, string keyword, int page, int pageSize)
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

            var model = _advertiseContractService.GetAllPaging(fromdate, todate, isSaleAdmin, isAccountant,A.KeyId, status, keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetAllStatisticPaging(string fromdate, string todate, int advertiserId, int page, int pageSize)
        {
            var id = advertiserId;
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var A = _advertiserService.GetBysId(userid);
            if(A.KeyId!=0)
            {
                id = A.KeyId;
            }
            var model = _advertiseContractService.GetAllStatisticPaging(fromdate, todate, id, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertisementContentViewModel adContentVm, AdvertiseContractViewModel advertiseContractVm)
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
                   var _adContent = _advertisementContentService.Add(adContentVm);
                   advertiseContractVm.AdvertisementContentFK = _adContent.KeyId;
                   _advertiseContractService.Add(advertiseContractVm);
                    
                }
                else
                {
                    _advertisementContentService.Update(adContentVm);
                    _advertiseContractService.Update(advertiseContractVm);
                }
                _advertiseContractService.Save();
                return new OkObjectResult(advertiseContractVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllRequestingNPaidContract()
        {
            var model = _advertiseContractService.GetAllRequestingNPaidContract();
            return new OkObjectResult(model);
        }
        
        
        [HttpGet]
        public IActionResult GetAdvertiserByStatistic()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var A = _advertiserService.GetBysId(userid);
            var model = _advertiserService.GetAdvertiserByStatistic(A.KeyId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllFutureSuccessContract(int id)
        {
            var model = _advertiseContractService.GetAllFutureContractByPositionId(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAdContentById(int id)
        {
            var model = _advertisementContentService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetAdPositionById(int id)
        {
            var model = _advertisementPositionService.GetById(id);
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


        [HttpPost]
        public IActionResult UpdateUnqualifiedAdContent(AdvertisementContentViewModel adContentVm)
        {
            _advertisementContentService.UpdateUnqualifiedAdContent(adContentVm);
            _advertisementContentService.Save();
            return new OkObjectResult("true");
        }

        [Authorize]
        [HttpPost]
        public IActionResult ImportFiles(IList<IFormFile> files)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var advertiser = new AdvertiserViewModel();
            if (Guid.TryParse(userid, out var guid))
            {
                advertiser = _advertiserService.GetBysId(userid);
            }

            if (files != null && files.Count > 0)
            {
                List<string> fileName = new List<string>();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var extension = Path.GetExtension(file.FileName);
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filename = (filename + extension).ToString();
                    string folder = _hostingEnvironment.WebRootPath + $@"\images\advertiser\" + advertiser.BrandName + "\\content";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, filename);
                    fileName.Add(Path.Combine($@"\images\advertiser\" + advertiser.BrandName + "\\content", filename).Replace($@"\", $@"/"));

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
                _advertisementContentService.Save();
                return new OkObjectResult(fileName);
            }
            return new NoContentResult();
        }
    }
}