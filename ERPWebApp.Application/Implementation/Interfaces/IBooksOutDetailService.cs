using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBooksOutDetailService : IDisposable
    {

        BooksOutDetailViewModel Add(BooksOutDetailViewModel BooksOutDetailViewModel);

        void Update(BooksOutDetailViewModel BooksOutDetailViewModel);

        List<BooksOutDetailViewModel> GetAll();

        PagedResult<BooksOutDetailViewModel> GetAllPaging(string keyword, int page, int pageSize);

        BooksOutDetailViewModel GetById(int id);

        List<BooksOutDetailViewModel> GetAllByBooksOutId(int id);
        bool Save();
    }
}
