using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    [Authorize]
    public class BookCategoryController : Controller
    {

        IBookCategoryService _bookCategoryService;
        IUnitOfWork _unitOfWork;
        public BookCategoryController(IBookCategoryService bookCategoryService, IUnitOfWork unitOfWork)
        {

            _bookCategoryService = bookCategoryService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API
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