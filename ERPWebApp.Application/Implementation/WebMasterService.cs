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

        public PagedResult<WebMasterViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
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
    }
}
