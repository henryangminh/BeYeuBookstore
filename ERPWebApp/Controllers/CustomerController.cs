﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerService _customerService;
        private readonly IUserService _userService;
        IUnitOfWork _unitOfWork;
        public CustomerController(IUserService userService,ICustomerService customerService, IUnitOfWork unitOfWork)
        {
            _customerService = customerService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(UserViewModel userVm)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (userVm != null)
                {
                    _userService.UpdateByCustomer(userVm);
                   
                    return new OkObjectResult(userVm);
                }
                else
                {
                    return new BadRequestResult();
                }
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _customerService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _customerService.GetById(id);
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
                _customerService.Delete(id);
                _customerService.Save();
                return new OkObjectResult(id);

            }


        }


    }
}