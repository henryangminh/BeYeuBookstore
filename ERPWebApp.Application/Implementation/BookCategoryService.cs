using AutoMapper;
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
            var bookCategory = Mapper.Map<BookCategoryViewModel, BookCategory>(BookCategoryViewModel);
            _bookCategoryRepository.Add(bookCategory);
            _unitOfWork.Commit();
            return BookCategoryViewModel;
        }

        public void Delete(int id)
        {
            _bookCategoryRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BookCategoryViewModel> GetAll()
        {
            var query = _bookCategoryRepository.FindAll();
            var data = new List<BookCategoryViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BookCategory, BookCategoryViewModel>(item);
                data.Add(_data);
            }
            return data;
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
            return Mapper.Map<BookCategory, BookCategoryViewModel>(_bookCategoryRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(BookCategoryViewModel BookCategoryViewModel)
        {
            var temp = _bookCategoryRepository.FindById(BookCategoryViewModel.KeyId);
            if (temp != null)
            {
                temp.BookCategoryName = BookCategoryViewModel.BookCategoryName;
            }
        }
    }
}
