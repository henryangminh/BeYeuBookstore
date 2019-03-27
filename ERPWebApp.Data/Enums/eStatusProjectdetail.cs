using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Data.Enums
{
    public enum eStatusProjectdetail
    {
        create, // từ lúc khởi tạo đến lúc chưa tạo SO
        buildingBoq1, // từ lúc tạo so đến lúc trúng thầu
        buildedBoq1, // xác nhận trúng thầu
        RequesteCreateBoq2, // giám đốc yêu cầu tạo BOQ2
        buildingBoq2, // từ lúc Yêu đến khi hoàn thành boq2.
        buildedBoq2, // đã hoàn thành boq2 nhưng giám đốc chưa xác nhận
        building, // giám đốc đã xác nhận,xong boq2 đến lúc hoàn thành
        finish, // hoàn thành
        cancel // không trúng thầu
    }
}
