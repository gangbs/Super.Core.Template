using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Query2Exp
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}
