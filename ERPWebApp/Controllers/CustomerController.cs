using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerService _customerService;
        IUnitOfWork _unitOfWork;
        public CustomerController(ICustomerService customerService, IUnitOfWork unitOfWork)
        {
            _customerService = customerService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}