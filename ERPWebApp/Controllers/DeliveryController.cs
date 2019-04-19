using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class DeliveryController : Controller
    {

        IDeliveryService _deliveryService;
        IUnitOfWork _unitOfWork;
        public DeliveryController(IDeliveryService deliveryService, IUnitOfWork unitOfWork)
        {
            _deliveryService = deliveryService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int status, string keyword, int page, int pageSize)
        {
            var model = _deliveryService.GetAllPaging(status, keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _deliveryService.GetById(id);
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
                _deliveryService.Delete(id);
                _deliveryService.Save();
                return new OkObjectResult(id);

            }


        }
    }
}