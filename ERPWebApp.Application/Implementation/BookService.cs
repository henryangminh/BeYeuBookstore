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

        public List<BookViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<BookViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<BookViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public BookViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(BookViewModel BookViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
