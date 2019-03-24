using BeYeuBookstore.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels.Acc
{
    public class PermissionViewModel
    {
        public int KeyId { get; set; }

        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }
        public bool CanConfirm { set; get; }

        public RoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}
