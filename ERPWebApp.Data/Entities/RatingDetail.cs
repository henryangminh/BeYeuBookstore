using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class RatingDetail : DomainEntity<int>, IDateTracking
    {
        public RatingDetail() { }
        public RatingDetail(int keyId, int bookFK, int customerFK,double rating ,string comment, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            BookFK = bookFK;
            CustomerFK = customerFK;
            Rating = rating;
            Comment = comment;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int BookFK { get; set; }
        public int CustomerFK { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? DateCreated {get;set;}
        public DateTime? DateModified { get; set; }

        public virtual Book BookFKNavigation { get; set; }
        public virtual Customer CustomerFKNavigation { get; set; }
    }
}
