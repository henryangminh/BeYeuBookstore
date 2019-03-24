using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeYeuBookstore.Data.Entities
{
    public partial class ArAccountsReceivable:DomainEntity<int>,IDateTracking
    {
        public ArAccountsReceivable()
        {
            Amount = 0;
        }

        public ArAccountsReceivable(int keyId,int? transactNo, DateTime? transaction_Date, DateTime? dateCreated, DateTime? dateModified,
            string arNo, int? customerFk, int? projectId, string recordCode, int? poNo, string description, decimal amount, 
            string warehouseFk, string department, string type)
        {
            KeyId = keyId;
            TransactNo = transactNo;
            Transaction_Date = transaction_Date;
            DateCreated = dateCreated;
            DateModified = dateModified;
            ArNo = arNo;
            CustomerFk = customerFk;
            ProjectId = projectId;
            RecordCode = recordCode;
            PoNo = poNo;
            Description = description;
            Amount = amount;
            WarehouseFk = warehouseFk;
            Department = department;
            Type = type;
        }

        public int? TransactNo { get; set; }
        public DateTime? Transaction_Date { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string ArNo { get; set; }
        public int? CustomerFk { get; set; }
        public int? ProjectId { get; set; }
        public string RecordCode { get; set; }
        public int? PoNo { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string WarehouseFk { get; set; }
        public string Department { get; set; }
        [StringLength(2)]
        public string Type { get; set; }
        
    }
}
