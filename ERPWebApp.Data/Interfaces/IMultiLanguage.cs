using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Interfaces
{
   public  interface IMultiLanguage<T>
    { 
        T LanguageId { get; set; }

    }
}
