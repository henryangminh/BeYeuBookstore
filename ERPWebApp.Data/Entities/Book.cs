using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Book : DomainEntity<int>,IDateTracking
    {
        public Book() { }

        public Book(int keyId, string bookTitle, string author, int bookCategoryFK, int merchantFK, bool isPaperback, decimal unitPrice, int? length, int? height, int? width, int pageNumber, string description, string img ,int quantity,int ratingNumber, double rating, Status status ,DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            BookTitle = bookTitle;
            Author = author;
            BookCategoryFK = bookCategoryFK;
            MerchantFK = merchantFK;
            this.isPaperback = isPaperback;
            UnitPrice = unitPrice;
            Length = length;
            Height = height;
            Width = width;
            PageNumber = pageNumber;
            Description = description;
            Img = img;
            DateCreated = dateCreated;
            Quantity = quantity;
            RatingNumber = ratingNumber;
            Rating = rating;
            Status = status;
            DateModified = dateModified;
        }

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
        public virtual Merchant MerchantFKNavigation { get; set; }
        //
        public virtual BookCategory BookCategoryFKNavigation { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<RatingDetail> RatingDetails { get; set; }
        public virtual ICollection<BooksInDetail> BooksInDetails { get; set; }
        public virtual ICollection<BooksOutDetail> BooksOutDetails { get; set; }
    }
}
