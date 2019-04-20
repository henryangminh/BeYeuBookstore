using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class DeliveryController : Controller
    {

        IDeliveryService _deliveryService;
        IMerchantService _merchantService;
        IInvoiceDetailService _invoiceDetailService;
        IUnitOfWork _unitOfWork;
        public DeliveryController(IInvoiceDetailService invoiceDetailService,IMerchantService merchantService , IDeliveryService deliveryService, IUnitOfWork unitOfWork)
        {
            _merchantService = merchantService;
            _deliveryService = deliveryService;
            _invoiceDetailService = invoiceDetailService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int status, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _deliveryService.GetAllPaging(M.KeyId,status, keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _deliveryService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetInvoiceDetailByInvoiceId(int invoiceId)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _invoiceDetailService.GetAllByInvoiceIdAndMerchantId(invoiceId, M.KeyId);
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