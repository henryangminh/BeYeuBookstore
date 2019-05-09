using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    
    public class BookCategoryController : Controller
    {

        IBookCategoryService _bookCategoryService;
        IAuthorizationService _authorizationService;
        IUnitOfWork _unitOfWork;
        public BookCategoryController(IAuthorizationService authorizationService, IBookCategoryService bookCategoryService, IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.BookCategory, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }

        #region AJAX API
        [Authorize]
        [HttpPost]
        public IActionResult SaveEntity(BookCategoryViewModel bookCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (bookCategoryVm.KeyId == 0)
                {

                    _bookCategoryService.Add(bookCategoryVm);
                }
                else
                {
                    _bookCategoryService.Update(bookCategoryVm);
                }
                _bookCategoryService.Save();
                return new OkObjectResult(bookCategoryVm);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _bookCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _bookCategoryService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _bookCategoryService.GetById(id);
            return new OkObjectResult(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _bookCategoryService.Delete(id);
                _bookCategoryService.Save();
                return new OkObjectResult(id);

            }
        }
        #endregion
    }
}