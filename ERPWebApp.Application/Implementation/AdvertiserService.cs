using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class AdvertiserService : IAdvertiserService
    {
        private IRepository<Advertiser, int> _advertiserRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertiserService(IRepository<Advertiser, int> advertiserRepository, IUnitOfWork unitOfWork)
        {
            _advertiserRepository = advertiserRepository;
            _unitOfWork = unitOfWork;

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
