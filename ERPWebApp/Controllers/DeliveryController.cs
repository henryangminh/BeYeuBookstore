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

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {

        IDeliveryService _deliveryService;
        IMerchantService _merchantService;
        IInvoiceDetailService _invoiceDetailService;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public DeliveryController(IAuthorizationService authorizationService,IInvoiceDetailService invoiceDetailService,IMerchantService merchantService , IDeliveryService deliveryService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _merchantService = merchantService;
            _deliveryService = deliveryService;
            _invoiceDetailService = invoiceDetailService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.Delivery, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int status, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _deliveryService.GetAllPaging(M.KeyId, status, fromdate, todate, keyword, page, pageSize);
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
        public IActionResult UpdateStatus(DeliveryViewModel deliVm )
        {
            if(deliVm!=null)
            {
                _deliveryService.UpdateStatus(deliVm);
                _deliveryService.Save();
                return new OkObjectResult(deliVm);
            }
            return new BadRequestResult();

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