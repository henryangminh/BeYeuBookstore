using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class MerchantContractViewModel
    {
        public int KeyId { get; set; }
        public string ContractLink { get; set; }
        public int MerchantFK { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual MerchantViewModel MerchantFKNavigation { get; set; }
    }
}
