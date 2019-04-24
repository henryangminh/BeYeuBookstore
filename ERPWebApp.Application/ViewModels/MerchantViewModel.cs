using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class MerchantViewModel
    {
        public int KeyId { get; set; }
        public Guid UserFK { get; set; }
        /// <summary>
        /// Người liên hệ trực tiếp
        /// </summary>
        public string DirectContactName { get; set; }
        /// <summary>
        /// SĐT Người liên hệ trực tiếp
        /// </summary>
        public string Hotline { get; set; }
        public string MerchantCompanyName { get; set; }
        /// <summary>
        /// Địa chỉ in trên hóa đơn
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        public string ContactAddress { get; set; }
        public string BussinessRegisterId { get; set; }
        public string TaxId { get; set; }
        public string Website { get; set; }
        /// <summary>
        /// Người đại diện trên hợp đồng
        /// </summary>
        public string LegalRepresentative { get; set; }
        public string MerchantBankingName { get; set; }
        //Tên ngân hàng
        public string Bank { get; set; }
        //Chi nhánh
        public string BankBranch { get; set; }

        public string BussinessRegisterLinkImg { get; set; }
        /// <summary>
        /// đăng ký thông báo tài khoản ngân hàng tại sở kế hoạch đầu tư/ mẫu 08_mst
        /// </summary>
        public string BankAccountNotificationLinkImg { get; set; }
        public string TaxRegisterLinkImg { get; set; }

        //Kiểm duyệt lần 1 được duyệt bán
        public Status Status { get; set; }
        //Quy mô của Merchant
        public Scale Scales { get; set; }

        /// <summary>
        /// Ngày thành lập
        /// </summary>
        public DateTime EstablishDate { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual UserViewModel UserBy { get; set; }
    }
}
