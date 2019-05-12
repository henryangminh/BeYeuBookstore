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
    public class BooksInDetailService : IBooksInDetailService
    {
        private IRepository<BooksInDetail, int> _booksInDetailRepository;
        private IUnitOfWork _unitOfWork;
        public BooksInDetailService(IRepository<BooksInDetail, int> booksInDetailRepository, IUnitOfWork unitOfWork)
        {
            _booksInDetailRepository = booksInDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public BooksInDetailViewModel Add(BooksInDetailViewModel BooksInDetailViewModel)
        {
            var book = Mapper.Map<BooksInDetailViewModel, BooksInDetail>(BooksInDetailViewModel);
            _booksInDetailRepository.Add(book);
            _unitOfWork.Commit();
            return BooksInDetailViewModel;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BooksInDetailViewModel> GetAll()
        {
            var query = _booksInDetailRepository.FindAll();
            var data = new List<BooksInDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksInDetail, BooksInDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }
        public List<BooksInDetailViewModel> GetAllByBooksInId(int id)
        {
            var query = _booksInDetailRepository.FindAll(x=>x.BookFKNavigation, x => x.BookFKNavigation.BookCategoryFKNavigation);
            query = query.Where(x => x.BooksInFK == id);
            var data = new List<BooksInDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksInDetail, BooksInDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<BooksInDetailViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public BooksInDetailViewModel GetById(int id)
        {
            return Mapper.Map<BooksInDetail, BooksInDetailViewModel>(_booksInDetailRepository.FindById(id, p=>p.BookFKNavigation, p=>p.BooksInFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(BooksInDetailViewModel BookViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
