using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{

    public class BeyeuBookstoreController : Controller
    {
        IBookService _bookService;
        public BeyeuBookstoreController(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Shopping()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        #region AJAX API
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _bookService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllQuantity(int quantity)
        {
            var model = _bookService.GetAll(quantity);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _bookService.GetById(id);
            return new OkObjectResult(model);
        }
        #endregion
    }
}