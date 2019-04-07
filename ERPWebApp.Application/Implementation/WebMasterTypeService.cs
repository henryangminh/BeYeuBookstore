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

        public List<WebMasterTypeViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(WebMasterTypeViewModel WebMasterTypeViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
