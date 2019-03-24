using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change from db context,  Nó đảm bảo sự toàn vẹ của các kết nối.
        /// </summary>
        bool Commit();
    }
}
