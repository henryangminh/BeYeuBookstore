using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class BookViewModel
    {
        public int KeyId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public int BookCategoryFK { get; set; }
        public int MerchantFK { get; set; }
        /// <summary>
        /// có phải là bìa mềm hay không
        /// </summary>
        public bool isPaperback { get; set; }
        public decimal UnitPrice { get; set; }
        public int? Length { get; set; }
        /// <summary>
        /// Chiều cao, có thể để trống
        /// </summary>
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int PageNumber { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Đánh giá
        /// </summary>
        public int RatingNumber { get; set; }
        public double Rating { get; set; }
        public Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual MerchantViewModel MerchantFKNavigation { get; set; }
        //
        public virtual BookCategoryViewModel BookCategoryFKNavigation { get; set; }
    }
}
