using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.Constants
{
  public  class const_BoqSalesOrderStatus
    {
        public const int create = 1;
        public const int RequestedComfirmBoq1 = 2;
        public const int NoComfirmBoq1 = 3;
        public const int ConfirmedBoq1 = 4;
        public const int CompletedBoq1 = 5; // trúng thầu
        public const int FinishBOQ1 = 6;
        public const int RequesteCreateBoq2 = 7;
        public const int buildingBoq2 = 8;
        public const int RequesteConfirmBoq2 = 9;  // nhân viên yêu cầu xác nhận BOQ2
        public const int buildedBoq2 = 10;

        public const int building = 11; //đang xây đựng 
        public const int Settlement = 12;
        public const int Finish = 13; // đã hoàn tất
        public const int cancelled = 14; //
    }
}
