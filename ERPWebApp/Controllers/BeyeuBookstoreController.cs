using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Extensions;
using BeYeuBookstore.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BeYeuBookstore.Controllers
{

    public class BeyeuBookstoreController : Controller
    {
        IBookService _bookService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        public BeyeuBookstoreController(IBookService bookService, SignInManager<User> signInManager, IUserService userService)
        {
            _bookService = bookService;
            _signInManager = signInManager;
            _userService = userService;
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

        public IActionResult CheckOut()
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
        public IActionResult GetAllPaging(string txtSearch, int BookCategoryId, int? From, int? To, int page, int pageSize)
        {
            var model = _bookService.GetAllPaging(txtSearch, BookCategoryId, From, To, page, pageSize);
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

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var urlSuccess = Url.Action("Index", "BeYeuBookstore");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userService.GetByEmailAsync(model.Email);
                    if (user.UserTypeFK == 3)
                    {
                        HttpContext.Session.Set("IsLogin", true);
                        HttpContext.Session.Set("User", user);
                        return new OkObjectResult(urlSuccess);
                    }
                    return new OkObjectResult("permission");
                }
                if (result.IsLockedOut)
                {
                    return new OkObjectResult("locked");
                }
                return new OkObjectResult("failed");
            }
            return new OkObjectResult("invalid");
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("IsLogin");
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("CartSession");
            return new OkObjectResult(Url.Action("Index","BeyeuBookstore"));
        }
        #endregion
    }
}