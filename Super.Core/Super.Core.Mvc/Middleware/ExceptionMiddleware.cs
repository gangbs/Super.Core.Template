using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Super.Core.Mvc.Filter;
using Super.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Super.Core.Mvc.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        readonly ILogger<object> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionLogModel> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                var loggerfactory = (ILoggerFactory)context.RequestServices.GetService(typeof(ILoggerFactory));
                var logger = loggerfactory.CreateLogger($"{e.TargetSite.DeclaringType}.{e.TargetSite.Name}");

                var res = new ErrorResponseModel { code = (int)HttpStatusCode.BadRequest, message = e.Message, detail = e };

                logger.LogError(JsonConvert.SerializeObject(res));
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
            }
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }

}
