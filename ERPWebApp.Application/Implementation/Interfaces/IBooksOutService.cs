using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBooksOutService : IDisposable
    {
        BooksOutViewModel Add(BooksOutViewModel BooksOutViewModel);

        void Update(BooksOutViewModel BooksOutViewModel);

        List<BooksOutViewModel> GetAll();

        PagedResult<BooksOutViewModel> GetAllPaging(int mId, int? merchantId, string fromdate, string todate, string keyword, int page, int pageSize);

        BooksOutViewModel GetById(int id);
        int GetLastest();
        bool Save();
    }
}
