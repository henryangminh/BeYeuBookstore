using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class BookCategoryViewModel
    {
        public int KeyId { get; set; }
        public Status Status { get; set; }
        public string BookCategoryName { get; set; }
    }
}
