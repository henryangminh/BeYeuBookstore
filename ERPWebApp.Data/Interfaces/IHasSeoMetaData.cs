using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Interfaces
{
   public interface IHasSeoMetaData
    { 
        string SeoPageTitle { set; get; } //chưa tiêu đề trang web
        string SeoAlias { set; get; } //chứa tên trang .html hiển thị
        string SeoKeywords { set; get; } // key search
        string SeoDescription { set; get; }
    }
}
