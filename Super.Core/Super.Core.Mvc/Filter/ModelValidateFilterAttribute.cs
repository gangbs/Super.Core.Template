using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class ModelValidateFilterAttribute : ActionFilterAttribute
    {
        readonly ILogger<ModelValidateFailLogModel> _logger;
        public ModelValidateFilterAttribute(ILogger<ModelValidateFailLogModel> logger)
        {
            this._logger = logger;
        }


        public IDictionary<string, object> RequestArguments { get; private set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                //WriteErrorToLog(context);
                context.Result = new BadRequestObjectResult(new ModelValidateFailResponse(context.ModelState));
                return;
            }

            this.RequestArguments = context.ActionArguments;

            // do something before the action executes
            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set
        }


        //private void WriteErrorToLog(ActionExecutingContext context)
        //{
        //    var msg = new ApiErrorLogMsg();
        //    var action = (ControllerActionDescriptor)context.ActionDescriptor;
        //    msg.ControllerName = action.ControllerName;
        //    msg.ActionName = action.ActionName;
        //    msg.HttpMethod = context.HttpContext.Request.Method;
        //    msg.Path = context.HttpContext.Request.Path;
        //    msg.User = context.HttpContext.User.Identity.Name;
        //    msg.RequestJson = context.ActionArguments.ToJson();
        //    msg.Msg = context.ModelState.GetAllErrorMessage();
        //    Log4NetWriter.GetInstance().ApiErrorLog(msg);
        //}

    }

    public class ModelValidateFailResponse : ErrorResponseModel
    {
        readonly ModelStateDictionary _modelState;
        public ModelValidateFailResponse(ModelStateDictionary modelState)
        {
            this._modelState = modelState;
            this.code = (int)HttpStatusCode.BadRequest;
            GenErrorMsg();
        }

        private void GenErrorMsg()
        {
            if (!string.IsNullOrEmpty(this.message))
                return;

            if (this._modelState.ErrorCount < 1)
                return;

            List<string> lstMsg = new List<string>();
            List<object> lstdetail = new List<object>();
            foreach (var item in this._modelState)
            {

                if (item.Value.ValidationState != ModelValidationState.Invalid)
                    continue;

                if (item.Value.Errors.Count < 1)
                    continue;

                string fieldName = string.IsNullOrEmpty(item.Key) ? "model" : item.Key;

                string msg = $"{fieldName}:{item.Value.Errors[0].ErrorMessage}";

                lstMsg.Add(msg);

                lstdetail.Add(new { field = fieldName, errorMessage = item.Value.Errors[0].ErrorMessage, value = item.Value.RawValue });
            }

            this.message = string.Join(';', lstMsg);
            this.detail = lstdetail;
        }
    }

    public class ModelValidateFailLogModel
    {

    }

}
