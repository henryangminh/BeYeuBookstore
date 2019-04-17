using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        IBookCategoryService _bookCategoryService;
        IMerchantService _merchantService;
        IUnitOfWork _unitOfWork;
        public BookController(IMerchantService merchantService ,IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork =unitOfWork;
            _merchantService = merchantService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(BookViewModel bookVm)
        {
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
                    if (bookVm.KeyId == 0)
                    {

                        _bookService.Add(bookVm);
                    }
                    else
                    {
                        _bookService.Update(bookVm);
                    }
                    _bookCategoryService.Save();
                    return new OkObjectResult(bookVm);
                }
                else
                {
                    return new BadRequestResult();
                }
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize)
        {
            var model = _bookService.GetAllPaging(fromdate, todate, keyword, bookcategoryid, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllBookCategory()
        {
            var model = _bookCategoryService.GetAll();
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _bookService.GetById(id);
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

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _bookService.Delete(id);
                _bookService.Save();
                return new OkObjectResult(id);

            }


        }
    }
}