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
    public class AdvertisementPositionService : IAdvertisementPositionService
    {
        private IRepository<AdvertisementPosition, int> _advertisementPositionRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertisementPositionService(IRepository<AdvertisementPosition, int> advertisementPositionRepository, IUnitOfWork unitOfWork)
        {
            _advertisementPositionRepository = advertisementPositionRepository;
            _unitOfWork = unitOfWork;

        }
        public AdvertisementPositionViewModel Add(AdvertisementPositionViewModel advertisementPositionViewModel)
        {
            var AdvertisementPosition = Mapper.Map<AdvertisementPositionViewModel, AdvertisementPosition>(advertisementPositionViewModel);
            var  a = _advertisementPositionRepository.AddReturn(AdvertisementPosition);
            advertisementPositionViewModel.KeyId = a.KeyId;
            _unitOfWork.Commit();
            return advertisementPositionViewModel;
        }

        public void Delete(int id)
        {
            _advertisementPositionRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<AdvertisementPositionViewModel> GetAll()
        {
            var query = _advertisementPositionRepository.FindAll();
            var data = new List<AdvertisementPositionViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertisementPosition, AdvertisementPositionViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<AdvertisementPositionViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AdvertisementPositionViewModel> GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var query = _advertisementPositionRepository.FindAll();
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => (x.PageUrl.ToUpper().Contains(keysearch)));

            }
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == (Status)status);
            }
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<AdvertisementPositionViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertisementPosition, AdvertisementPositionViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<AdvertisementPositionViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public AdvertisementPositionViewModel GetById(int id)
        {
            return Mapper.Map<AdvertisementPosition, AdvertisementPositionViewModel>(_advertisementPositionRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(AdvertisementPositionViewModel AdvertisementPositionViewModel)
        {
            var temp = _advertisementPositionRepository.FindById(AdvertisementPositionViewModel.KeyId);
            if (temp != null)
            {
                temp.AdvertisePrice = AdvertisementPositionViewModel.AdvertisePrice;
                temp.Height = AdvertisementPositionViewModel.Height;
                temp.Width = AdvertisementPositionViewModel.Width;
                temp.PageUrl = AdvertisementPositionViewModel.PageUrl;
                temp.IdOfPosition = AdvertisementPositionViewModel.IdOfPosition;
                temp.Status = AdvertisementPositionViewModel.Status;
            }
        }
    }
}
