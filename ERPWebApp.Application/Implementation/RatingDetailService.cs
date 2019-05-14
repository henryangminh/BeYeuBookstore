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
    public class RatingDetailService : IRatingDetailService
    {
        private IRepository<RatingDetail, int> _ratingDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RatingDetailService(IRepository<RatingDetail, int> ratingDetailRepository, IUnitOfWork unitOfWork)
        {
            _ratingDetailRepository = ratingDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public RatingDetailViewModel Add(RatingDetailViewModel ratingDetailViewModel)
        {
            var ratingDetail = Mapper.Map<RatingDetailViewModel, RatingDetail>(ratingDetailViewModel);
            _ratingDetailRepository.Add(ratingDetail);
            _unitOfWork.Commit();
            return ratingDetailViewModel;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<RatingDetailViewModel> GetAll()
        {
            var query = _ratingDetailRepository.FindAll();
            var data = new List<RatingDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<RatingDetail, RatingDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<RatingDetailViewModel> GetAllByBookId(int id)
        {
            var query = _ratingDetailRepository.FindAll(x => x.BookFK == id, x => x.CustomerFKNavigation, x => x.CustomerFKNavigation.UserBy);
            var data = new List<RatingDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<RatingDetail, RatingDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<RatingDetailViewModel> GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public RatingDetailViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }


        public void Update(RatingDetailViewModel ratingDetailViewModel)
        {
            var temp = _ratingDetailRepository.FindById(ratingDetailViewModel.KeyId);
            if (temp != null)
            {
                temp.Rating = ratingDetailViewModel.Rating;
                temp.Comment = ratingDetailViewModel.Comment;
            }
        }

        public double CalculateBookRatingByBookId(int id)
        {
            var query = _ratingDetailRepository.FindAll(x => x.BookFK == id);
            var count = query.Count();
            double rating = 0;
            foreach (var item in query)
            {
                rating += item.Rating;
            }
            return Math.Round(rating / count, 1);
        }
    }
}
