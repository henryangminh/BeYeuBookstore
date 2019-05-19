using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class BookService : IBookService
    {
        private IRepository<Book, int> _bookRepository;
        private IUnitOfWork _unitOfWork;
        public BookService(IRepository<Book, int> bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }
        public BookViewModel Add(BookViewModel BookViewModel)
        {
            var book = Mapper.Map<BookViewModel, Book>(BookViewModel);
            _bookRepository.Add(book);
            _unitOfWork.Commit();
            return BookViewModel;
        }

        public void Delete(int id)
        {
            _bookRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BookViewModel> GetAll()
        {
            var query = _bookRepository.FindAll();
            var data = new List<BookViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Book, BookViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<BookViewModel> GetAllByMerchantId(int id)
        {
            var query = _bookRepository.FindAll(x=>x.MerchantFK==id);
            var data = new List<BookViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Book, BookViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<BookViewModel> GetAll(int quantity)
        {
            var query = _bookRepository.FindAll();
            query = query.Where(x => x.Status == Status.Active && x.Quantity > 0).OrderByDescending(x => x.KeyId).Take(quantity);
            var data = new List<BookViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Book, BookViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<BookViewModel> GetAllPaging(int mId,int? merchantId, string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize)
        {
            var query = _bookRepository.FindAll(x=>x.BookCategoryFKNavigation, x=>x.MerchantFKNavigation);
            query = query.OrderByDescending(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderByDescending(x => x.KeyId).Where(x => (x.BookTitle.ToUpper().Contains(keysearch)) || (x.MerchantFKNavigation.MerchantCompanyName.ToUpper().Contains(keysearch)));

            }
            if(merchantId!=0)
            {
                query = query.OrderByDescending(x => x.KeyId).Where(x => x.MerchantFK==merchantId);
 
            }
            if (!string.IsNullOrEmpty(fromdate))
            {
                var date = DateTime.Parse(fromdate);
                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime _fromdate = date.Date + ts;
                query = query.Where(x => x.DateCreated >= _fromdate);

            }
            if (!string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(todate);
                TimeSpan ts = new TimeSpan(23, 59, 59);
                DateTime _todate = date.Date + ts;
                query = query.Where(x => x.DateCreated <= _todate);

            }
            if (bookcategoryid != 0)
            {
                query = query.Where(x => x.BookCategoryFK == bookcategoryid);
            }
            if (mId != 0)
            {
                query = query.Where(x => x.MerchantFK == mId);
            }
          
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<BookViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Book, BookViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<BookViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public PagedResult<BookViewModel> GetAllPaging(string txtSearch, int BookCategoryId, int? From, int? To, int? MerchantId, int? OrderBy, int? Order, int page, int pageSize)
        {
            var query = _bookRepository.FindAll(x => x.BookCategoryFKNavigation, x => x.MerchantFKNavigation);
            query = query.Where(x => x.Status == Data.Enums.Status.Active && x.Quantity > 0);
            query = query.OrderBy(x => x.KeyId);

            if (txtSearch != "" && txtSearch != null)
            {
                query = query.Where(x => x.BookTitle.Contains(txtSearch) || x.Description.Contains(txtSearch));
            }
            if (BookCategoryId != 0)
            {
                query = query.Where(x => x.BookCategoryFK == BookCategoryId);
            }

            if (From != null)
            {
                query = query.Where(x => x.UnitPrice >= From);
            }

            if (To != null)
            {
                query = query.Where(x => x.UnitPrice <= To);
            }

            if (MerchantId != null)
            {
                query = query.Where(x => x.MerchantFK == MerchantId);
            }

            if (OrderBy != null && Order != null)
            {
                switch (OrderBy)
                {
                    case 1:
                        if (Order == 1)
                        {
                            query = query.OrderBy(x => x.DateCreated);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.DateCreated);
                        }
                        break;
                    case 2:
                        if (Order == 1)
                        {
                            query = query.OrderBy(x => x.UnitPrice);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice);
                        }
                        break;
                    default:
                        break;
                }
            }

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<BookViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Book, BookViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<BookViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public BookViewModel GetById(int id)
        {
            return Mapper.Map<Book, BookViewModel>(_bookRepository.FindById(id, p => p.BookCategoryFKNavigation, p => p.MerchantFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }
        
        public void Update(BookViewModel BookViewModel)
        {
            var temp = _bookRepository.FindById(BookViewModel.KeyId);
            if (temp != null)
            {
                temp.Author = BookViewModel.Author;
                temp.BookCategoryFK = BookViewModel.BookCategoryFK;
                temp.BookTitle = BookViewModel.BookTitle;
                temp.Description = BookViewModel.Description;
                temp.Height = BookViewModel.Height;
                temp.Length = BookViewModel.Length;
                temp.Width = BookViewModel.Width;
                temp.PageNumber = BookViewModel.PageNumber;
                temp.isPaperback = BookViewModel.isPaperback;
                temp.UnitPrice = BookViewModel.UnitPrice;
                temp.Img = BookViewModel.Img;
                temp.Rating = BookViewModel.Rating;
                temp.RatingNumber = BookViewModel.RatingNumber;
                temp.Quantity = BookViewModel.Quantity;
            }
        }

        public void UpdateBookRating(double rating, int id)
        {
            var temp = _bookRepository.FindById(id);
            if (temp != null)
            {
                temp.Rating = rating;
            }
        }

        public void UpdateBookStatus(int bookId, int status)
        {
            var temp = _bookRepository.FindById(bookId);
            if (temp != null)
            {
                temp.Status = (Status)status;
            }
        }


        public void UpdateBookQtyByBooksIn(int bookid, int qty)
        {
            var temp = _bookRepository.FindById(bookid);
            if (temp != null)
            {
                temp.Quantity = temp.Quantity+qty;
            }
        }

        public void UpdateBookQtyByBooksOut(int bookid, int qty)
        {
            var temp = _bookRepository.FindById(bookid);
            if (temp != null)
            {
                if (temp.Quantity >= qty)
                {
                    temp.Quantity = temp.Quantity - qty;
                }
                
            }
        }
    }
}
