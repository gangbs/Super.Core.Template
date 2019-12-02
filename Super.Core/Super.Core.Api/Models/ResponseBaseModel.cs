using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Core.Api.Models
{
    public class ResponseBaseModel
    {
    }

    public class ErrorResponseModel : ResponseBaseModel
    {
        public int code { get; set; }

        public string message { get; set; }

        public object detail { get; set; }
    }
}
