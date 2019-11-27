using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.EF
{
    public interface IUnitOfWork
    {
        SaveResult SaveChanges();
    }
}
