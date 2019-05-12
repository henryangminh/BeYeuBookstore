using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IBookCategoryService : IDisposable
    {
        BookCategoryViewModel Add(BookCategoryViewModel BookCategoryViewModel);

        void Update(BookCategoryViewModel BookCategoryViewModel);

        void Delete(int id);

        List<BookCategoryViewModel> GetAll();

        PagedResult<BookCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<BookCategoryViewModel> GetAll(int id);

        BookCategoryViewModel GetById(int id);

        bool Save();
    }
}
