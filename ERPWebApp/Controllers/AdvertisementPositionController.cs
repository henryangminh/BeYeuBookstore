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
    public class AdvertisementPositionController : Controller
    {
        IAdvertisementPositionService _advertisementPositionService;
        IAuthorizationService _authorizationService;
        IAdvertiseContractService _advertiseContractService;
        IUnitOfWork _unitOfWork;
        public AdvertisementPositionController(IAdvertiseContractService advertiseContractService ,IAuthorizationService authorizationService, IAdvertisementPositionService advertisementPositionService, IUnitOfWork unitOfWork)
        {
            _advertiseContractService = advertiseContractService;
            _authorizationService = authorizationService;
            _advertisementPositionService = advertisementPositionService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.AdvertisementPosition, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _advertisementPositionService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertisementPositionViewModel advertisementPositionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (advertisementPositionVm.KeyId == 0)
                {

                    _advertisementPositionService.Add(advertisementPositionVm);
                }
                else
                {
                    _advertisementPositionService.Update(advertisementPositionVm);
                }
                _advertisementPositionService.Save();
                return new OkObjectResult(advertisementPositionVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var model = _advertisementPositionService.GetAllPaging(status, keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetAllFutureContract(int id)
        {
            var model = _advertiseContractService.GetAllContractByPositionId(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _advertisementPositionService.GetById(id);
            return new OkObjectResult(model);
        }


    }
}