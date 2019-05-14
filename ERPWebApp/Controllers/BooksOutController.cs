using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Services;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class BooksOutController : Controller
    {

        IRatingDetailService _ratingDetailService;
        IBookService _bookService;
        IBooksInService _booksInService;
        IBooksOutService _booksOutService;
        IBooksInDetailService _booksInDetailService;
        IBookCategoryService _bookCategoryService;
        IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        IMerchantService _merchantService;
        private readonly IHostingEnvironment _hostingEnvironment;
        IUnitOfWork _unitOfWork;
        public BooksOutController(IBooksInDetailService booksInDetailService, IBooksInService booksInService, IRatingDetailService ratingDetailService, IEmailService emailService, IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment, IMerchantService merchantService, IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _booksInDetailService = booksInDetailService;
            _booksInService = booksInService;
            _ratingDetailService = ratingDetailService;
            _emailService = emailService;
            _authorizationService = authorizationService;
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork = unitOfWork;
            _merchantService = merchantService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllMerchantInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _merchantService.GetAllByBook(M.KeyId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int mId, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _booksOutService.GetAllPaging(mId, M.KeyId, fromdate, todate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

    }


}