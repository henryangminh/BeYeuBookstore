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
    public class BooksOutService : IBooksOutService
    {
        private IRepository<BooksOut, int> _booksOutRepository;
        private IUnitOfWork _unitOfWork;
        public BooksOutService(IRepository<BooksOut, int> booksOutRepository, IUnitOfWork unitOfWork)
        {
            _booksOutRepository = booksOutRepository;
            _unitOfWork = unitOfWork;
        }
        public BooksOutViewModel Add(BooksOutViewModel BooksOutViewModel)
        {
            var book = Mapper.Map<BooksOutViewModel, BooksOut>(BooksOutViewModel);
            _booksOutRepository.Add(book);
            _unitOfWork.Commit();
            return BooksOutViewModel;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

            public List<BooksOutViewModel> GetAll()
        {
            var query = _booksOutRepository.FindAll(p=>p.MerchantFKNavigation);
            var data = new List<BooksOutViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<BooksOut, BooksOutViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<BooksOutViewModel> GetAllPaging(int mId, int? merchantId, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _booksOutRepository.FindAll(p => p.MerchantFKNavigation);
            var data = new List<BooksOutViewModel>();
            if (query.Count() != 0)
            {

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
                if (mId != 0)
                {
                    query = query.Where(x => x.MerchantFK == mId);
                }

                if (merchantId != 0)
                {
                    query = query.Where(x => x.MerchantFK == merchantId);
                }




                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                foreach (var item in query)
                {
                    var _data = Mapper.Map<BooksOut, BooksOutViewModel>(item);
                    data.Add(_data);
                }
            }
            int totalRow = query.Count();
            var paginationSet = new PagedResult<BooksOutViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }


        public int GetLastest()
        {
            var invoice = _booksOutRepository.FindAll();
            invoice = invoice.OrderByDescending(x => x.KeyId);
            var _invoice = invoice.First();
            return _invoice.KeyId;
        }

        public BooksOutViewModel GetById(int id)
        {
            return Mapper.Map<BooksOut, BooksOutViewModel>(_booksOutRepository.FindById(id, p => p.MerchantFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(BooksOutViewModel BooksOutViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
