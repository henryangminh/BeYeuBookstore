using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class BookCategoryService : IBookCategoryService
    {
        private IRepository<BookCategory, int> _bookCategoryRepository;
        private IUnitOfWork _unitOfWork;
        public BookCategoryService(IRepository<BookCategory, int> bookCategoryRepository, IUnitOfWork unitOfWork)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _unitOfWork = unitOfWork;

        }
        public BookCategoryViewModel Add(BookCategoryViewModel BookCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<BookCategoryViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<BookCategoryViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<BookCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public BookCategoryViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(BookCategoryViewModel BookCategoryViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
