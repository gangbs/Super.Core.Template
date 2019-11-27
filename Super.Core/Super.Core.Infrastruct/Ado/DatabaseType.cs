using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Super.Core.Infrastruct.Ado
{
    public enum DatabaseType
    {
        [Description("sqlserver")]
        sqlserver = 1,
        [Description("mysql")]
        mysql = 2,
        [Description("oracle")]
        oracle = 3
    }
}
