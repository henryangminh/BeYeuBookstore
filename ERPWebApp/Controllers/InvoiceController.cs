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
        ICustomerService _customerService;
        IBookService _bookService;
        public InvoiceController(IDeliveryService deliveryService,IAuthorizationService authorizationService, IInvoiceDetailService invoiceDetailService, IInvoiceService invoiceService, IUnitOfWork unitOfWork, ICustomerService customerService, IBookService bookService)
        {
            _deliveryService = deliveryService;
            _authorizationService = authorizationService;
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _bookService = bookService;
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
        public IActionResult SaveEntity(InvoiceViewModel invoiceVm, List<InvoiceDetailViewModel> invoiceDetailVms)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
                var c = _customerService.GetBysId(userid);
                if (c.KeyId != 0)
                {
                    //var book = _bookService.GetById()
                    foreach (var item in invoiceDetailVms)
                    {
                        var book = _bookService.GetById(item.BookFK);
                        if (item.Qty > book.Quantity)
                            return new OkObjectResult("quantity");
                    }

                    if (invoiceVm.DeliAddress == "" || invoiceVm.DeliAddress == null)
                    {
                        invoiceVm.DeliAddress = c.UserBy.Address;
                    }

                    if (invoiceVm.DeliContactName == "" || invoiceVm.DeliContactName == null)
                    {
                        invoiceVm.DeliContactName = c.UserBy.FullName;
                    }

                    if (invoiceVm.DeliContactHotline == "" || invoiceVm.DeliContactHotline == null)
                    {
                        invoiceVm.DeliContactHotline = c.UserBy.PhoneNumber;
                    }

                    invoiceVm.CustomerFK = c.KeyId;

                    _invoiceService.Add(invoiceVm);
                    _invoiceService.Save();
                    var invoice = _invoiceService.GetLastest();
                    foreach (var item in invoiceDetailVms)
                    {
                        item.InvoiceFK = invoice;
                        _invoiceDetailService.Add(item);
                        var book = _bookService.GetById(item.BookFK);
                        book.Quantity = book.Quantity - item.Qty;
                        _bookService.Update(book);
                    }

                    var deli = invoiceDetailVms.GroupBy(x => x.MerchantFK).Select(x => new DeliveryViewModel()
                    {
                        InvoiceFK = invoice,
                        DeliveryStatus = Const_DeliStatus.UnConfirmed,
                        OrderPrice = x.Sum(y => y.SubTotal),
                        ShipPrice = 25000,
                        MerchantFK = x.Key,
                    }).ToList();

                    foreach (var item in deli)
                    {
                        _deliveryService.Add(item);
                    }

                    _invoiceService.Save();
                    HttpContext.Session.Remove("CartSession");
                    return new OkObjectResult("true");
                }
                return new OkObjectResult("customer");
            }
        }
    }
}