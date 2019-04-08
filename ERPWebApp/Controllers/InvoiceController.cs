using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class InvoiceController : Controller
    {

        IInvoiceService _invoiceService;
        IUnitOfWork _unitOfWork;
        public InvoiceController(IInvoiceService invoiceService, IUnitOfWork unitOfWork)
        {
            _invoiceService = invoiceService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}