using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Data.EF;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Models.AccountViewModels;
using BeYeuBookstore.Utilities.Constants;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class AdvertiserController : Controller
    {
        IAdvertiserService _advertiserService;
        IAuthorizationService _authorizationService;
        IUnitOfWork _unitOfWork;
        private readonly ERPDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        public AdvertiserController(IAuthorizationService authorizationService ,IAdvertiserService advertiserService, IUnitOfWork unitOfWork, UserManager<User> userManager, IUserService userService, ERPDbContext context)
        {
            _authorizationService = authorizationService;
            _advertiserService = advertiserService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.Advertiser, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        #region AJAX API
        public async Task<IActionResult> Register(RegisterViewModel model, UserViewModel userViewModel, AdvertiserViewModel advertiserViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Data.Entities.User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = userViewModel.FullName,
                    Address = userViewModel.Address,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    PhoneNumber = userViewModel.PhoneNumber,
                    Gender = Data.Enums.Gender.Other,
                    Status = Data.Enums.Status.InActive,
                    UserTypeFK = Const_UserType.Advertiser,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme, nameof(AccountController.ConfirmEmail), "Advertiser");

                    var content = "Hãy nhấp vào link này để xác nhận tài khoản quảng cáo Bé Yêu Bookstore: " + callbackUrl;

                    SendConfirmEmail(model.Email, content);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role
                    var advertiser = new Advertiser();
                    advertiser.UserFK = user.Id;
                    advertiser.BrandName = advertiserViewModel.BrandName;
                    advertiser.UrlToBrand = advertiserViewModel.UrlToBrand;
                    await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role
                    _context.Advertisers.Add(advertiser);
                    _context.SaveChanges();

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

                new MailHelper().SendMail(toEmailAddress, "Xác nhận email tài khoản quảng cáo Bé Yêu Bookstore", MailContent);
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
            if (result.Succeeded)
            {
                user.Status = Data.Enums.Status.Active;
                await _userService.UpdateAsync(Mapper.Map<User, UserViewModel>(user));
                await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role
                _context.SaveChanges();
            }
            return new RedirectResult(Url.Action("Index", "BeyeuBookstore"));
        }

        [HttpPost]
        public IActionResult SaveEntity(AdvertiserViewModel advertiserVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (advertiserVm.KeyId == 0)
                {

                    _advertiserService.Add(advertiserVm);
                }
                else
                {
                    _advertiserService.Update(advertiserVm);
                }
                _advertiserService.Save();
                return new OkObjectResult(advertiserVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var model = _advertiserService.GetAllPaging(status,keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _advertiserService.GetById(id);
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
                _advertiserService.Delete(id);
                _advertiserService.Save();
                return new OkObjectResult(id);

            }
        }
        #endregion
    }
}