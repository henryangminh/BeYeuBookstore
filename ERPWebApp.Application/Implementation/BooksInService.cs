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
    public class BooksInService : IBooksInService
    {
        private IRepository<BooksIn, int> _booksInRepository;
        private IUnitOfWork _unitOfWork;
        public BooksInService(IRepository<BooksIn, int> booksInRepository, IUnitOfWork unitOfWork)
        {
            _booksInRepository = booksInRepository;
            _unitOfWork = unitOfWork;
        }
        public BooksInViewModel Add(BooksInViewModel booksInViewModel)
        {
            var book = Mapper.Map<BooksInViewModel, BooksIn>(booksInViewModel);
            _booksInRepository.Add(book);
            _unitOfWork.Commit();
            return booksInViewModel;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BooksInViewModel> GetAll()
        {
            var query = _booksInRepository.FindAll(x=>x.MerchantFKNavigation);
            var data = new List<BooksInViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksIn, BooksInViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<BooksInViewModel> GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _booksInRepository.FindAll(p => p.MerchantFKNavigation);
            query = query.OrderByDescending(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => x.MerchantFKNavigation.MerchantCompanyName.ToUpper().Contains(keysearch));

            }

            if (!string.IsNullOrEmpty(fromdate))
            {
                var date = DateTime.Parse(fromdate);
                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime _fromdate = date.Date + ts;
                query = query.Where(x => x.DateCreated >= _fromdate);

            }
            if (!string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(todate);
                TimeSpan ts = new TimeSpan(23, 59, 59);
                DateTime _todate = date.Date + ts;
                query = query.Where(x => x.DateCreated <= _todate);

            }


            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<BooksInViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksIn, BooksInViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<BooksInViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public BooksInViewModel GetById(int id)
        {
            return Mapper.Map<BooksIn, BooksInViewModel>(_booksInRepository.FindById(id, p => p.MerchantFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(BooksInViewModel BookViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
