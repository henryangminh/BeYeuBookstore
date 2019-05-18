using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBookService : IDisposable
    {
        BookViewModel Add(BookViewModel BookViewModel);

        void Update(BookViewModel BookViewModel);

        void Delete(int id);

        List<BookViewModel> GetAll();
        List<BookViewModel> GetAllByMerchantId(int id);

        PagedResult<BookViewModel> GetAllPaging(int mId, int? merchantId, string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize);

        PagedResult<BookViewModel> GetAllPaging(string txtSearch, int BookCategoryId, int? From, int? To, int? MerchantId, int? OrderBy, int? Order, int page, int pageSize);

        List<BookViewModel> GetAll(int id);

        BookViewModel GetById(int id);
        void UpdateBookRating(double rating, int id);

        void UpdateBookQtyByBooksIn(int bookid, int qty);
        void UpdateBookStatus(int bookId, int status);
        void UpdateBookQtyByBooksOut(int bookid, int qty);
        bool Save();
    }
}
