using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class AdvertiseContractViewModel
    {
        public int KeyId { get; set; }
        public int? AdvertisementContentFK { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public decimal ContractValue { get; set; }
        public decimal Deposite { get; set; }
        public string Note { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }


        public virtual AdvertisementContentViewModel AdvertisementContentFKNavigation { get; set; }
    }
}
