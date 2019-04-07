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
        IUnitOfWork _unitOfWork;
        public BookController(IBookService bookService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _unitOfWork=unitOfWork;
    }
        public IActionResult Index()
        {
            return View();
        }
    }
}