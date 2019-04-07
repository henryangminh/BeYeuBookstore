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

        public List<WebMasterViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(WebMasterViewModel WebMasterViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
