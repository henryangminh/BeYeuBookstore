using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class AdvertiseContract:DomainEntity<int>,IDateTracking
    {
        public AdvertiseContract() { }

        public AdvertiseContract(int keyId, int advertisementContentFK, DateTime dateStart, DateTime dateFinish, decimal contractValue, bool paid, Status status, DateTime? dateCreated, DateTime? dateModified)
        {
            keyId = KeyId;
            AdvertisementContentFK = advertisementContentFK;
            DateStart = dateStart;
            DateFinish = dateFinish;
            ContractValue = contractValue;
            Paid = paid;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int AdvertisementContentFK { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public decimal ContractValue { get; set; }
        [DefaultValue(false)]
        public bool Paid { get; set; }
        public Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual AdvertisementContent AdvertisementContentFKNavigation { get; set; }
    }
}
