using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Super.Core.Infrastruct.Ado
{
    public interface IBatch<T> :IDisposable
    {
        string TableName { get; }
        //DbConnection DbConn { get; }
        int BatchInsert(List<T> lstData);
        Task<int> BatchInsertAsync(List<T> lstData);
    }
}
