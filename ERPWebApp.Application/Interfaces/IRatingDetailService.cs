using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IRatingDetailService : IDisposable
    {
        RatingDetailViewModel Add(RatingDetailViewModel ratingDetailViewModel);

        void Update(RatingDetailViewModel ratingDetailViewModel);

        void Delete(int id);

        List<RatingDetailViewModel> GetAll();

        PagedResult<RatingDetailViewModel> GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize);

        List<RatingDetailViewModel> GetAll(int id);

        RatingDetailViewModel GetById(int id);

        bool Save();
    }
}
