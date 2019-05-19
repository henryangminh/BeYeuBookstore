using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Extensions;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class CartController : Controller
    {
        IBookService _bookService;
        IUnitOfWork _unitOfWork;

        public CartController(IBookService bookService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _unitOfWork = unitOfWork;
        }

        //[Route("Cart.chtml", Name ="Cart")]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("Checkout.chtml", Name = "Cart")]
        /*
        public IActionResult CheckOut()
        {
            return View();
        }
        */

        #region AJAX API
        /// <summary>
        /// Get list cart
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            ReloadCart();
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session == null)
            {
                session = new List<CartViewModel>();
            }
            return new OkObjectResult(session);
        }

        public IActionResult ReloadCart()
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session == null)
            {
                session = new List<CartViewModel>();
            }
            else
            {
                foreach (var item in session)
                {
                    item.Book = _bookService.GetById(item.Book.KeyId);
                    if (item.Book.Quantity == 0)
                        item.Quantity = 0;
                    if (item.Quantity > item.Book.Quantity)
                        item.Quantity = item.Book.Quantity;
                }
                HttpContext.Session.Set("CartSession", session);
            }
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Clear all cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("CartSession");
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add To Cart
        /// </summary>
        /// <param name="BookId"></param>
        /// <param name="quantity"></param>
        /// <param name="update">if true -> quantity = quantity, else quantity += quantity </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int BookId, int quantity, bool update)
        {
            //Get book detail
            var book = _bookService.GetById(BookId);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");
            if (session != null)
            {
                //Covert string to list object
                bool hasChanged = false;

                //Check exist with item BookId
                if (session.Any(x => x.Book.KeyId == BookId))
                {
                    foreach (var item in session)
                    {
                        if (item.Book.KeyId == BookId)
                        {
                            if (update)
                                item.Quantity = quantity;
                            else
                                item.Quantity += quantity;
                            item.UnitPrice = item.Book.UnitPrice;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new CartViewModel()
                    {
                        Book = book,
                        Quantity = quantity,
                        UnitPrice = book.UnitPrice
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set("CartSession", session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<CartViewModel>();
                cart.Add(new CartViewModel()
                {
                    Book = book,
                    Quantity = quantity,
                    UnitPrice = book.UnitPrice
                });
                HttpContext.Session.Set("CartSession", cart);
            }

            return new OkObjectResult(BookId);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int BookId)
        {
            var session = HttpContext.Session.Get<List<CartViewModel>>("CartSession");

            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Book.KeyId == BookId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set("CartSession", session);
                }
                return new OkObjectResult(BookId);
            }
            return new EmptyResult();
        }
        #endregion
    }
}