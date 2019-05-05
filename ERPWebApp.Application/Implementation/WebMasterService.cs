using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class WebMasterService : IWebMasterService
    {
        private IRepository<WebMaster, int> _webMasterRepository;
        private IUnitOfWork _unitOfWork;
        public WebMasterService(IRepository<WebMaster, int> webMasterRepository, IUnitOfWork unitOfWork)
        {
            _webMasterRepository = webMasterRepository;
            _unitOfWork = unitOfWork;
        }
        public WebMasterViewModel Add(WebMasterViewModel WebMasterViewModel)
        {
            var webMaster = Mapper.Map<WebMasterViewModel, WebMaster>(WebMasterViewModel);
            _webMasterRepository.Add(webMaster);
            _unitOfWork.Commit();
            return WebMasterViewModel;
        }

        public void Delete(int id)
        {
            _webMasterRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<WebMasterViewModel> GetAll()
        {
            var query = _webMasterRepository.FindAll();
            var data = new List<WebMasterViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<WebMaster, WebMasterViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<WebMasterViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<WebMasterViewModel> GetAllPaging(int? type, int? status, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _webMasterRepository.FindAll(p => p.UserBy, x=>x.WebMasterTypeFKNavigation);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => x.UserBy.FullName.ToUpper().Contains(keysearch));

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
            if (type.HasValue)
            {
                query = query.Where(x => x.WebMasterTypeFK == type);
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.UserBy.Status == (Status)status);
            }

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<WebMasterViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<WebMaster, WebMasterViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<WebMasterViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public WebMasterViewModel GetById(int id)
        {
            return Mapper.Map<WebMaster, WebMasterViewModel>(_webMasterRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(WebMasterViewModel WebMasterViewModel)
        {
            var temp = _webMasterRepository.FindById(WebMasterViewModel.KeyId);
            if (temp != null)
            {
                temp.WebMasterTypeFK = WebMasterViewModel.WebMasterTypeFK;
            }
        }

        public WebMasterViewModel GetBysId(string id)
        {
            var query = _webMasterRepository.FindAll(p => p.UserBy );
            var _data = new WebMasterViewModel();
            foreach (var item in query)
            {
                if ((item.UserFK.ToString()) == id)
                {
                    _data = Mapper.Map<WebMaster, WebMasterViewModel>(item);
                }
            }
            return _data;
        }
    }
}
