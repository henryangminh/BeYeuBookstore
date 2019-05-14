using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class BooksOutDetailService : IBooksOutDetailService
    {
        private IRepository<BooksOutDetail, int> _booksOutDetailRepository;
        private IUnitOfWork _unitOfWork;
        public BooksOutDetailService(IRepository<BooksOutDetail, int> booksOutDetailRepository, IUnitOfWork unitOfWork)
        {
            _booksOutDetailRepository = booksOutDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public BooksOutDetailViewModel Add(BooksOutDetailViewModel BooksOutDetailViewModel)
        {
            var book = Mapper.Map<BooksOutDetailViewModel, BooksOutDetail>(BooksOutDetailViewModel);
            _booksOutDetailRepository.Add(book);
            _unitOfWork.Commit();
            return BooksOutDetailViewModel;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BooksOutDetailViewModel> GetAll()
        {
            var query = _booksOutDetailRepository.FindAll();
            var data = new List<BooksOutDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksOutDetail, BooksOutDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<BooksOutDetailViewModel> GetAllByBooksOutId(int id)
        {
            var query = _booksOutDetailRepository.FindAll(x => x.BookFKNavigation, x => x.BookFKNavigation.BookCategoryFKNavigation);
            query = query.Where(x => x.BooksOutFK == id);
            var data = new List<BooksOutDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksOutDetail, BooksOutDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<BooksOutDetailViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public BooksOutDetailViewModel GetById(int id)
        {
            return Mapper.Map<BooksOutDetail, BooksOutDetailViewModel>(_booksOutDetailRepository.FindById(id, p => p.BookFKNavigation, p => p.BooksOutFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(BooksOutDetailViewModel BooksOutDetailViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
