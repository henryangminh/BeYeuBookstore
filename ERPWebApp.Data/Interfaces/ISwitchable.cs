using ERPWebApp.Data.eEnum;
using System;
using System.Collections.Generic;
using System.Text;
namespace ERPWebApp.Data.Interfaces
{
    // là Status?
   public interface ISwitchable
    {
       Status  Status { set; get; }
    }
}
