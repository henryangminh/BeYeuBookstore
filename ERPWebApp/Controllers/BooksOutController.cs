using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Services;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class BooksOutController : Controller
    {

        IRatingDetailService _ratingDetailService;
        IBookService _bookService;
        IBooksInService _booksInService;
        IBooksOutService _booksOutService;
        IBooksInDetailService _booksInDetailService;
        IBooksOutDetailService _booksOutDetailService;
        IBookCategoryService _bookCategoryService;
        IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        IMerchantService _merchantService;
        private readonly IHostingEnvironment _hostingEnvironment;
        IUnitOfWork _unitOfWork;
        public BooksOutController(IBooksOutDetailService booksOutDetailService, IBooksOutService booksOutService, IBooksInDetailService booksInDetailService, IBooksInService booksInService, IRatingDetailService ratingDetailService, IEmailService emailService, IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment, IMerchantService merchantService, IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _booksOutDetailService = booksOutDetailService;
            _booksOutService = booksOutService;
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
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.BooksIssue, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
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


        [HttpGet]
        public IActionResult GetMerchantInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            if (Guid.TryParse(userid, out var guid))
            {
                var model = _merchantService.GetBysId(userid);
                return new OkObjectResult(model);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public IActionResult GetBookById(int id)
        {

            var model = _bookService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _booksOutService.GetById(id);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetAllBookByMerchantId()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);

            if (Guid.TryParse(userid, out var guid))
            {

                var M = _merchantService.GetBysId(userid);
                var model = _bookService.GetAllByMerchantId(M.KeyId);
                return new OkObjectResult(model);
            }
            return new BadRequestResult();
        }


        [HttpGet]
        public IActionResult GetAllDetailById(int id)
        {
            var model = _booksOutDetailService.GetAllByBooksOutId(id);
            return new OkObjectResult(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveEntity(List<BooksOutDetailViewModel> listBooksOutDetailVms)
        {
            var bookVm = new BooksOutViewModel();
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var model = new MerchantViewModel();
            if (Guid.TryParse(userid, out var guid))
            {
                model = _merchantService.GetBysId(userid);

            }
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (model != null)
                {
                    bookVm.MerchantFK = model.KeyId;
                    _booksOutService.Add(bookVm);
                    _booksOutService.Save();
                    var booksOutFK = _booksOutService.GetLastest();
                    foreach (var i in listBooksOutDetailVms)
                    {
                        i.BooksOutFK = booksOutFK;
                        _booksOutDetailService.Add(i);
                        _bookService.UpdateBookQtyByBooksOut(i.BookFK, i.Qty);
                    }
                    _booksInDetailService.Save();
                    return new OkObjectResult(bookVm);
                }
                else
                {
                    return new BadRequestResult();
                }
            }
        }
    }


}