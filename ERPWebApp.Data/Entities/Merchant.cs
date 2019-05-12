using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Merchant : DomainEntity<int>, IDateTracking
    {
        public Merchant() { }
        public Merchant(int keyId, string directContactName, string hotline,  string merchantCompanyName, string address, string contactAddress, string bussinessRegisterId, string taxId, string website, string legalRepresentative, string merchantBankingName,  string bank, string bankBranch, string bussinessRegisterLinkImg, string bankAccountNotificationLinkImg, string taxRegisterLinkImg, Status status, Scale scales, DateTime establishDate, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            DirectContactName = directContactName;
            Hotline = hotline;
            MerchantCompanyName = merchantCompanyName;
            Address = address;
            ContactAddress = contactAddress;
            BussinessRegisterId = bussinessRegisterId;
            TaxId = taxId;
            Website = website;
            LegalRepresentative = legalRepresentative;
            MerchantBankingName = merchantBankingName;
            Bank = bank;
            BankBranch = bankBranch;
            BussinessRegisterLinkImg = bussinessRegisterLinkImg;
            BankAccountNotificationLinkImg = bankAccountNotificationLinkImg;
            TaxRegisterLinkImg = taxRegisterLinkImg;
            Status = status;
            Scales = scales;
            EstablishDate = establishDate;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }
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
        //Ngày thành lập
        public DateTime EstablishDate { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User UserBy { get; set; }
        //
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<MerchantContract> MerchantContracts { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<BooksIn> BooksIns { get; set; }
        public virtual ICollection<BooksOut> BooksOuts { get; set; }
    }
}
