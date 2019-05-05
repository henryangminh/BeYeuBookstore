using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class CartViewModel
    {
        public BookViewModel Book { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
