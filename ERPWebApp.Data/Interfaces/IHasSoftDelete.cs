using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.Interfaces
{
    public interface IHasSoftDelete
    {
        bool IsDelete { get; set; }
    }
}
