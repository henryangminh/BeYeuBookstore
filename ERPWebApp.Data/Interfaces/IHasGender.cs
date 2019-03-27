using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Interfaces
{
   public interface IHasGender
    {
        Gender? Gender { get; set; }
    }
}
