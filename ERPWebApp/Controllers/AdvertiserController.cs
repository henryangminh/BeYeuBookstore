using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class AdvertiserController : Controller
    {
        IAdvertiserService _advertiserService;
        IUnitOfWork _unitOfWork;
        public AdvertiserController(IAdvertiserService advertiserService, IUnitOfWork unitOfWork)
        {
            _advertiserService = advertiserService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertiserViewModel advertiserVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (advertiserVm.KeyId == 0)
                {

                    _advertiserService.Add(advertiserVm);
                }
                else
                {
                    _advertiserService.Update(advertiserVm);
                }
                _advertiserService.Save();
                return new OkObjectResult(advertiserVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(int status, string keyword, int page, int pageSize)
        {
            var model = _advertiserService.GetAllPaging(status,keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _advertiserService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _advertiserService.Delete(id);
                _advertiserService.Save();
                return new OkObjectResult(id);

            }


        }
    }
}