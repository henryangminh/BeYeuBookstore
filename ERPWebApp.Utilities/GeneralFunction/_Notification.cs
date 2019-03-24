using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.GeneralFunction
{
    public class _Notification
    {
        private static _Notification instance;
        public static _Notification Instance
        {
            get { if (instance == null) instance = new _Notification(); return instance; }
            set { instance = value; }
        }
        public string Project(string ConstructionId, string ConstructionName, string FullName)
        {
            return "Công trình " + ConstructionId + " | " + ConstructionName + " vừa mới được tạo bởi " + FullName;
        }
        public string ConfirmRequestBOQ1(string SOId, string FullName)
        {
            return "Bạn có yêu cầu xác nhận bản dự thầu " + SOId + " từ " + FullName;
        }
        public string ComfirmBOQ1(string SOId, string FullName)
        {
            return "Bản dự thầu " + SOId + " đã được xác nhận bởi " + FullName;
        }
        public string UnComfirmedBOQ1(string SOId, string FullName)
        {
            return "Bản dự thầu " + SOId + " không được xác nhận bởi " + FullName;
        }
        public string ComfirmCompleteBOQ1(string SOId, string FullName)
        {
            return "Bản dự thầu " + SOId + " đã được xác nhận trúng thầu bởi " + FullName;
        }
        public string ContractConfirmed(string SOId, string FullName)
        {
            return "Hợp đồng " + SOId + " đã được đưa vào nội bộ bởi " + FullName;
        }
        public string CodeConfirm()
        {
            return "Mã xác nhận reset mật khẩu vừa được gửi đến email của bạn.";
        }
        public string CreateBOQ2(string SOId, string FullName)
        {
            return "Bạn nhận được yêu cầu tạo biểu giá thi công của hợp đồng " + SOId + " từ " + FullName;
        }
        public string ConfirmRequestBOQ2(string SOId, string FullName)
        {
            return "Bạn có yêu cầu xác nhận bản biểu giá thi công " + SOId + " từ " + FullName;
        }
        public string ConfirmBOQ2(string SOId, string FullName)
        {
            return "Biểu giá thi công " + SOId + " vừa được xác nhận bởi " + FullName;
        }
        public string UnConfirmBOQ2(string SOId, string FullName)
        {
            return "Biểu giá thi công " + SOId + " không được xác nhận bởi " + FullName;
        }
    }
}
