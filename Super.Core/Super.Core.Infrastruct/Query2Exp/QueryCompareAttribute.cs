using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Query2Exp
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryCompareAttribute : Attribute
    {
        public readonly CompareType compare;
        public int Order { get; set; }
        public string FieldName { get; set; }

        public QueryCompareAttribute(CompareType compare)
        {
            this.compare = compare;
        }
    }
}
