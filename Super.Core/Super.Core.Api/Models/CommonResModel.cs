using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Core.Api.Models
{
    public class CommonResModel
    {
        public int code { get; set; }
        public string message { get; set; }
    }


    public class CommonResModel<T> : CommonResModel
    {
        public T data { get; set; }
    }
}
