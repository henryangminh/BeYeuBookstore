using ERPWebApp.Data.eEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.Interfaces
{
   public interface IHasGender
    {
        Gender? Gender { get; set; }
    }
}
