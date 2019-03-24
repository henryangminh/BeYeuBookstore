using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Interfaces
{
    /// <summary>
    /// Interface add vào table cần ngày tạo
    /// Interface dùng để nhóm các thuộc tính dùng chung và hay đi với nhau, tránh thiếu...
    /// và có thể triển khai da kế thừa
    /// </summary>
    public interface IDateTracking
    {
        DateTime? DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
