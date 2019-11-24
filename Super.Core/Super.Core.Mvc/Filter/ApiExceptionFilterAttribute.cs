using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Super.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Super.Core.Mvc.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        readonly ILogger<ExceptionLogModel> _logger;

        public ApiExceptionFilterAttribute(ILogger<ExceptionLogModel> logger)
        {
            this._logger = logger;
        }


        public override void OnException(ExceptionContext context)
        {
            //如果该异常已被处理则跳过
            if (context.ExceptionHandled) return;

            var action = (ControllerActionDescriptor)context.ActionDescriptor;
            var reqParam = context.Filters.FirstOrDefault(x => x.GetType() == typeof(ModelValidateFilterAttribute));

            //ApiErrorLogMsg msg = new ApiErrorLogMsg
            //{
            //    HttpMethod = context.HttpContext.Request.Method,
            //    ControllerName = action.ControllerName,
            //    ActionName = action.ActionName,
            //    Path = context.HttpContext.Request.Path,
            //    User = context.HttpContext.User.Identity.Name,
            //    Msg = context.Exception,
            //    RequestJson = reqParam != null ? ((PimsModelValidateFilterAttribute)reqParam).RequestArguments.ToJson() : null
            //};

            //Log4NetWriter.GetInstance().ApiErrorLog(msg);


            context.Result = new BadRequestObjectResult(new ErrorResponseModel { code = (int)HttpStatusCode.BadRequest, message = context.Exception.Message, detail = context.Exception });
        }
    }

    public class ExceptionLogModel
    {

    }

}
