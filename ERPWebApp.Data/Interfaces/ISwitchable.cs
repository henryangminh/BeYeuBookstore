using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;
namespace BeYeuBookstore.Data.Interfaces
{
    // là Status?
   public interface ISwitchable
    {
       Status  Status { set; get; }
    }
}
