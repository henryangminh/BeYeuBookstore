using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class AdStatisticViewModel
    {
        public string Advertiser { get; set; }
        public int NoOfContract { get; set; }
        public int NoOfSuccessContract { get; set; }
        public decimal TotalContractValuePeriod { get; set; }
        public decimal TotalContractValue { get; set; }
    }
}
