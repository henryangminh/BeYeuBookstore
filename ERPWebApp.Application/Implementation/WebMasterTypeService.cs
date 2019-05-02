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
    public class WebMasterTypeService : IWebMasterTypeService
    {
        private IRepository<WebMasterType, int> _webMasterTypeRepository;
        private IUnitOfWork _unitOfWork;
        public WebMasterTypeService(IRepository<WebMasterType, int> webMasterTypeRepository, IUnitOfWork unitOfWork)
        {
            _webMasterTypeRepository = webMasterTypeRepository;
            _unitOfWork = unitOfWork;
        }
        public WebMasterTypeViewModel Add(WebMasterTypeViewModel WebMasterTypeViewModel)
        {
            var webMasterType = Mapper.Map<WebMasterTypeViewModel, WebMasterType>(WebMasterTypeViewModel);
            _webMasterTypeRepository.Add(webMasterType);
            _unitOfWork.Commit();
            return WebMasterTypeViewModel;
        }

        public void Delete(int id)
        {
            _webMasterTypeRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<WebMasterTypeViewModel> GetAll()
        {
            var query = _webMasterTypeRepository.FindAll();
            var data = new List<WebMasterTypeViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<WebMasterType, WebMasterTypeViewModel>(item);
                data.Add(_data);
            }
            return data;
         
        }

        public List<WebMasterTypeViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<WebMasterTypeViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public WebMasterTypeViewModel GetById(int id)
        {
            return Mapper.Map<WebMasterType, WebMasterTypeViewModel>(_webMasterTypeRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(WebMasterTypeViewModel WebMasterTypeViewModel)
        {
            var temp = _webMasterTypeRepository.FindById(WebMasterTypeViewModel.KeyId);
            if (temp != null)
            {
                temp.WebMasterTypeName = WebMasterTypeViewModel.WebMasterTypeName;
            }
        }
    }
}
