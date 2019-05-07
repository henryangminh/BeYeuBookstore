using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class AdvertisementPositionController : Controller
    {
        IAdvertisementPositionService _advertisementPositionService;
        IUnitOfWork _unitOfWork;
        public AdvertisementPositionController(IAdvertisementPositionService advertisementPositionService, IUnitOfWork unitOfWork)
        {
            _advertisementPositionService = advertisementPositionService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
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
        public IActionResult GetById(int id)
        {
            var model = _advertisementPositionService.GetById(id);
            return new OkObjectResult(model);
        }


    }
}