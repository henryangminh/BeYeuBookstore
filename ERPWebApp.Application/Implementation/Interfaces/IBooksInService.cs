using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBooksInService : IDisposable
    {
        BooksInViewModel Add(BooksInViewModel booksInViewModel);

        void Update(BooksInViewModel BookViewModel);

        List<BooksInViewModel> GetAll();

        PagedResult<BooksInViewModel> GetAllPaging(int mId, int? merchantId, string fromdate,string todate, string keyword, int page, int pageSize);

        BooksInViewModel GetById(int id);
        int GetLastest();
        bool Save();
    }
}
