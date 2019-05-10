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

        PagedResult<BookViewModel> GetAllPaging(int? merchantId, string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize);

        PagedResult<BookViewModel> GetAllPaging(string txtSearch, int BookCategoryId, int? From, int? To, int page, int pageSize);

        List<BookViewModel> GetAll(int id);

        BookViewModel GetById(int id);
        void UpdateBookRating(double rating, int id);
        bool Save();
    }
}
