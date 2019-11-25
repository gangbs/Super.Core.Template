using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Super.Core.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogTestController : ControllerBase
    {
        //日志的类别默认为方法所在类的类名
        readonly ILogger<LogTestController> _logger;




        public LogTestController(ILogger<LogTestController> logger, IConfiguration config)
        {
            this._logger = logger;
        }

        //自定义日志类别
        //public LogTestController(ILoggerFactory loggerFactory)
        //{
        //    var log = loggerFactory.CreateLogger("自定义类别");
        //}


        public IActionResult Get()
        {
            this._logger.LogInformation("controller log test %n 123456");
            //this._logger.Log(LogLevel.Information, "");
            return this.Ok();
        }


    }
}