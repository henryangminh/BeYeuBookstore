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
    public class InvoiceController : Controller
    {

        IInvoiceService _invoiceService;
        IDeliveryService _deliveryService;
        IInvoiceDetailService _invoiceDetailService;
        IUnitOfWork _unitOfWork;
        IAuthorizationService _authorizationService;
        public InvoiceController(IDeliveryService deliveryService,IAuthorizationService authorizationService, IInvoiceDetailService invoiceDetailService, IInvoiceService invoiceService, IUnitOfWork unitOfWork)
        {
            _deliveryService = deliveryService;
            _authorizationService = authorizationService;
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.Receipt, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var model = _invoiceService.GetAllPaging(fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _invoiceService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllInvoiceDetailById(int id)
        {
            var model = _invoiceDetailService.GetAll(id);
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
                _invoiceService.Delete(id);
                _invoiceService.Save();
                return new OkObjectResult(id);

            }

        }

        [HttpPost]
        public IActionResult SaveEntity(InvoiceViewModel invoiceVm, List<InvoiceDetailViewModel> invoiceDetailVms, List<DeliveryViewModel> deliveryVms)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var invoice = _invoiceService.Add(invoiceVm);
                _invoiceService.Save();
                var invoiceId = _invoiceService.GetLastest();
                foreach (var item in invoiceDetailVms)
                {
                    item.InvoiceFK = invoiceId;
                    _invoiceDetailService.Add(item);
                }
                foreach (var _item in deliveryVms)
                {
                    _item.InvoiceFK = invoiceId;
                    _deliveryService.Add(_item);
                }
                    return new OkObjectResult(invoiceVm);
            }
        }
    }
}