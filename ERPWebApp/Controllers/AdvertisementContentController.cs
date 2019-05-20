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
    public class AdvertisementContentController : Controller
    {

        IAdvertisementContentService _advertisementContentService;
        IAdvertisementPositionService _advertisementPositionService;
        IAdvertiserService _advertiserService;
        IWebMasterService _webMasterService;
        private readonly IHostingEnvironment _hostingEnvironment;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public AdvertisementContentController(IAuthorizationService authorizationService, IWebMasterService webMasterService,IHostingEnvironment hostingEnvironment,IAdvertisementPositionService advertisementPositionService,IAdvertiserService advertiserService,IAdvertisementContentService advertisementContentService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
               _advertisementContentService = advertisementContentService;
            _advertisementPositionService = advertisementPositionService;
            _advertiserService = advertiserService;
            _hostingEnvironment = hostingEnvironment;
            _webMasterService = webMasterService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.AdvertisementContent, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertisementContentViewModel advertisementContentVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (advertisementContentVm.KeyId == 0)
                {

                    _advertisementContentService.Add(advertisementContentVm);
                }
                else
                {
                    _advertisementContentService.Update(advertisementContentVm);
                }
                _advertisementPositionService.Save();
                return new OkObjectResult(advertisementContentVm);
            }
        }


        [HttpGet]
        public IActionResult GetAllPaging(int? status,int? advertiserSort, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
           
            var A = _advertiserService.GetBysId(userid);
            var WM = _webMasterService.GetBysId(userid);
            bool? isAccountant=false;
            bool? isAdCensor=false;
            if (WM != null)
            {
                if (WM.WebMasterTypeFK == Const_WebmasterType.Accountant)
                {
                    isAccountant = true;
                }
                if (WM.WebMasterTypeFK == Const_WebmasterType.AdCensor)
                {
                    isAdCensor = true;
                }
            }
     
            var model = _advertisementContentService.GetAllPaging(isAdCensor, isAccountant, A.KeyId, status, advertiserSort, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAdvertiserInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var model = _advertiserService.GetBysId(userid);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllAdPosition()
        {
            var model = _advertisementPositionService.GetAll();
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
            var model = _advertisementContentService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllAdvertiser()
        {
            var model = _advertiserService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, int status, string note)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var WM = _webMasterService.GetBysId(userid);
            _advertisementContentService.UpdateStatus(id,WM.KeyId,status, note);
            _advertisementContentService.Save();
            return new OkObjectResult("true");
        }


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