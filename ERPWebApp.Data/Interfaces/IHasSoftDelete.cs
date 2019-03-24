using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Interfaces
{
    public interface IHasSoftDelete
    {
        bool IsDelete { get; set; }
    }
}
