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
    public class DeliveryService : IDeliveryService
    {
        private IRepository<Delivery, int> _deliveryRepository;
        private IUnitOfWork _unitOfWork;
        public DeliveryService(IRepository<Delivery, int> deliveryRepository, IUnitOfWork unitOfWork)
        {
            _deliveryRepository = deliveryRepository;
            _unitOfWork = unitOfWork;
        }
        public DeliveryViewModel Add(DeliveryViewModel DeliveryViewModel)
        {
            var delivery = Mapper.Map<DeliveryViewModel, Delivery>(DeliveryViewModel);
            _deliveryRepository.Add(delivery);
            _unitOfWork.Commit();
            return DeliveryViewModel;
        }

        public void Delete(int id)
        {
            _deliveryRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DeliveryViewModel> GetAll()
        {

            var query = _deliveryRepository.FindAll();
            var data = new List<DeliveryViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Delivery, DeliveryViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<DeliveryViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<DeliveryViewModel> GetAllPaging(int merchantId, int status, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _deliveryRepository.FindAll(x=>x.InvoiceFKNavigation, x=>x.MerchantFKNavigation, x => x.InvoiceFKNavigation.CustomerFKNavigation.UserBy);
            query = query.OrderBy(x => x.KeyId);

            if (merchantId != 0)
            {
                query = query.Where(x => x.MerchantFK == merchantId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.Where(x => (x.InvoiceFKNavigation.CustomerFKNavigation.UserBy.FullName.ToUpper().Contains(keysearch) || x.InvoiceFKNavigation.CustomerFKNavigation.UserBy.UserName.ToUpper().Contains(keysearch)));
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
            if (status!=0)
            {
                query = query.Where(x => x.DeliveryStatus == status);
            }

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<DeliveryViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Delivery, DeliveryViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<DeliveryViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public DeliveryViewModel GetById(int id)
        {
            return Mapper.Map<Delivery, DeliveryViewModel>(_deliveryRepository.FindById(id, x => x.InvoiceFKNavigation, x => x.MerchantFKNavigation, x => x.InvoiceFKNavigation.CustomerFKNavigation.UserBy));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(DeliveryViewModel DeliveryViewModel)
        {
            var temp = _deliveryRepository.FindById(DeliveryViewModel.KeyId);
            if (temp != null)
            {
                temp.DeliveryStatus = DeliveryViewModel.DeliveryStatus;
            }
        }

        public void UpdateStatus(DeliveryViewModel DeliveryViewModel)
        {
            var temp = _deliveryRepository.FindById(DeliveryViewModel.KeyId);
            if (temp != null)
            {
                temp.DeliveryStatus = DeliveryViewModel.DeliveryStatus;
                temp.Note = DeliveryViewModel.Note;
            }
        }
        
        public DeliveryViewModel GetByDeliveryByInvoiceAndMerchant(int InvoiceId, int MerchantId)
        {
            var query = _deliveryRepository.FindAll();
            var data = query.Where(x => x.InvoiceFK == InvoiceId && x.MerchantFK == MerchantId).FirstOrDefault();
            return Mapper.Map<Delivery, DeliveryViewModel>(data);
        }
    }
}
