using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { get; set; }
    }
}
