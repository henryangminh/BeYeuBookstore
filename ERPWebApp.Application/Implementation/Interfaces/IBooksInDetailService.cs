using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBooksInDetailService : IDisposable
    {

        BooksInDetailViewModel Add(BooksInDetailViewModel BooksInDetailViewModel);

        void Update(BooksInDetailViewModel BookViewModel);

        List<BooksInDetailViewModel> GetAll();
        List<BooksInDetailViewModel> GetAllByBooksInId(int id);

        PagedResult<BooksInDetailViewModel> GetAllPaging( string keyword, int page, int pageSize);

        BooksInDetailViewModel GetById(int id);

        bool Save();
    }
}
