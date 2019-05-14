using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class RatingDetailViewModel : DomainEntity<int>, IDateTracking
    {
        /*
        public RatingDetailViewModel() { }
        public RatingDetailViewModel(int bookFK, int customerFK,double rating ,string comment, DateTime? dateCreated, DateTime? dateModified)
        {
            BookFK = bookFK;
            CustomerFK = customerFK;
            Rating = rating;
            Comment = comment;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }
        */
        public int BookFK { get; set; }
        public int CustomerFK { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? DateCreated {get;set;}
        public DateTime? DateModified { get; set; }

        public virtual BookViewModel BookFKNavigation { get; set; }
        public virtual CustomerViewModel CustomerFKNavigation { get; set; }
    }
}
