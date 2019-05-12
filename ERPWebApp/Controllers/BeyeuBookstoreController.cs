using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Extensions;
using BeYeuBookstore.Models.AccountViewModels;
using BeYeuBookstore.Services;
using BeYeuBookstore.Utilities.Constants;
using Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BeYeuBookstore.Controllers
{
    public class BeyeuBookstoreController : Controller
    {
        IBookService _bookService;
        IInvoiceService _invoiceService;
        ICustomerService _customerService;
        IInvoiceDetailService _invoiceDetailService;
        IRatingDetailService _ratingDetailService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        public BeyeuBookstoreController(IBookService bookService, SignInManager<User> signInManager, IUserService userService, IInvoiceService invoiceService, ICustomerService customerService, IInvoiceDetailService invoiceDetailService, IRatingDetailService ratingDetailService, UserManager<User> userManager, IEmailSender emailSender)
        {
            _bookService = bookService;
            _signInManager = signInManager;
            _userService = userService;
            _invoiceService = invoiceService;
            _customerService = customerService;
            _invoiceDetailService = invoiceDetailService;
            _ratingDetailService = ratingDetailService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            if (!HttpContext.Session.Get<Boolean>("IsLogin"))
                return View();
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public IActionResult Shopping()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            if (!HttpContext.Session.Get<Boolean>("IsLogin"))
                return View();
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            if (HttpContext.Session.Get<Boolean>("IsLogin"))
                return View();
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public IActionResult OrderHistory()
        {
            if (HttpContext.Session.Get<Boolean>("IsLogin"))
                return View(GetAllInvoiceByCustomerId(_customerService.GetBysId(HttpContext.Session.Get<User>("User").Id.ToString()).KeyId));
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public IActionResult OrderDetails(int? id)
        {
            if (HttpContext.Session.Get<Boolean>("IsLogin"))
            {
                if (id == null)
                    return View();
                InvoiceViewModel invoiceViewModel = _invoiceService.GetById((int)id);
                List<InvoiceDetailViewModel> invoiceDetailViewModel = _invoiceDetailService.GetAll((int)id);
                return View(new Tuple<InvoiceViewModel, List<InvoiceDetailViewModel>>(invoiceViewModel, invoiceDetailViewModel));
            }
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public IActionResult BookDetail(int? id)
        {
            if (id == null)
                return View();
            var book = _bookService.GetById((int)id);
            if (book == null)
                return View();
            var rating = _ratingDetailService.GetAllByBookId(book.KeyId);
            return View(new Tuple<BookViewModel, List<RatingDetailViewModel>>(book, rating));
        }

        public IActionResult WaitingConfirmation()
        {
            return View();
        }

        public IActionResult ConfirmationError()
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
        public async Task<IActionResult> LogOutAsync()
        {
            HttpContext.Session.Remove("IsLogin");
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("CartSession");
            await _signInManager.SignOutAsync();
            ////  Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return new OkObjectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        public List<InvoiceViewModel> GetAllInvoiceByCustomerId(int id)
        {
            return _invoiceService.GetAllInvoiceByCustomerId(id);
        }
        
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, UserViewModel userViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Data.Entities.User {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = userViewModel.FullName,
                    Address = userViewModel.Address,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    PhoneNumber = userViewModel.PhoneNumber,
                    Gender = Data.Enums.Gender.Other,
                    Status=Data.Enums.Status.InActive,
                    UserTypeFK= Const_UserType.Customer,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme, nameof(AccountController.ConfirmEmail), "BeyeuBookstore");

                    var content = "Hãy nhấp vào link này để xác nhận tài khoản Bé Yêu Bookstore: " + callbackUrl;

                    SendConfirmEmail(model.Email, content);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return new RedirectResult(Url.Action("WaitingConfirmation", "BeyeuBookstore"));
                }
            }

            // If we got this far, something failed, redisplay form
            return new RedirectResult(Url.Action("SignUp", "BeyeuBookstore"));
        }

        [HttpPost]
        public IActionResult CheckEmailExist(string email)
        {
            var model = _userService.checkExistEmail(email);
            return new OkObjectResult(model);
        }

        public void SendConfirmEmail(string toEmailAddress, string content)
        {
            try
            {
                string MailContent = content;

                new MailHelper().SendMail(toEmailAddress, "Xác nhận email tải khoản Bé Yêu Bookstore", MailContent);
            }
            catch (Exception ex)
            {
                
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new RedirectResult(Url.Action("ConfirmationError", "BeyeuBookstore"));
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) user.Status = Data.Enums.Status.Active;
            await _userService.UpdateAsync(Mapper.Map<User, UserViewModel>(user));
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }
        #endregion
    }
}