using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Data.Enums
{
    public enum eLevelAccess { View, Edit, Confirm }
    public class UserAccess
    {
        public static string GetName(eLevelAccess action)
        {
            switch (action)
            {
                case eLevelAccess.View: return "View"; // chỉ được xem
                case eLevelAccess.Edit: return "Edit"; // chỉ được chỉnh sủa, thêm mới
                case eLevelAccess.Confirm: return "Confirm"; // vừa được thêm xóa sữa vừa được  xác nhận.
            }
            return string.Empty;
        }
    }
}
