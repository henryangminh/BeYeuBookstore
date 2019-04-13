using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        IBookCategoryService _bookCategoryService;
        IUnitOfWork _unitOfWork;
        public BookController(IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork =unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
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
    }
}